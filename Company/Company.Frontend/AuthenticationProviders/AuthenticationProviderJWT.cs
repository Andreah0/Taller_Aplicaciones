using Company.Frontend.Helpers;
using Company.Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Company.Frontend.AuthenticationProviders;

public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthenticationProviderJWT> _logger;
    private readonly string _tokenKey = "TOKEN_KEY";
    private readonly AuthenticationState _anonimous;

    public AuthenticationProviderJWT(
        ProtectedLocalStorage protectedLocalStorage,
        HttpClient httpClient,
        ILogger<AuthenticationProviderJWT> logger)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _httpClient = httpClient;
        _logger = logger;
        _anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // devolvemos anónimo inmediatamente para no bloquear la carga
        _ = TryInitializeFromStorageAsync();
        return Task.FromResult(_anonimous);
    }

    private async Task TryInitializeFromStorageAsync()
    {
        try
        {
            var stored = await _protectedLocalStorage.GetAsync<string>(_tokenKey);
            if (stored.Success && !string.IsNullOrEmpty(stored.Value))
            {
                var authState = BuildAuthenticationState(stored.Value);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo inicializar token desde ProtectedLocalStorage.");
        }
    }

    private AuthenticationState BuildAuthenticationState(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var claims = ParseClaimsFromJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al construir AuthenticationState desde token");
            return _anonimous;
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJWT(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwt.Claims;
    }

    public async Task LoginAsync(string token)
    {
        try
        {
            await _protectedLocalStorage.SetAsync(_tokenKey, token);
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoginAsync falló");
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _protectedLocalStorage.DeleteAsync(_tokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LogoutAsync falló");
            throw;
        }
    }
}