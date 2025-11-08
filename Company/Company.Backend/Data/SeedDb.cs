using Company.Backend.UnitsOfWork.Interfaces;
using Company.Shared.Entities;
using Company.Shared.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Company.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesFullAsync();
        await CheckCountriesAsync();
        await CheckEmployeesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("1010", "Andrea", "Higuita", "andrea@yopmail.com", "311 255 4999", "Calle Luna Calle Sol", UserType.Admin);
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Address = address,
                Document = document,
                City = _context.Cities.FirstOrDefault(),
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
        }

        return user;
    }

    private async Task CheckCountriesFullAsync()
    {
        if (_context.Countries.Any())
        {
            Console.WriteLine("Countries ya existentes, no se ejecuta el .sql");
            return;
        }

        var sqlFilePath = Path.Combine("Data", "CountriesStatesCities.sql");

        if (!File.Exists(sqlFilePath))
        {
            Console.WriteLine($"No se encontró el archivo: {sqlFilePath}");
            return;
        }

        Console.WriteLine("Ejecutando CountriesStatesCities.sql...");

        var script = await File.ReadAllTextAsync(sqlFilePath);

        var batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        var connection = (SqlConnection)_context.Database.GetDbConnection();
        var wasClosed = connection.State == System.Data.ConnectionState.Closed;

        if (wasClosed)
            await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            foreach (var batch in batches)
            {
                var commandText = batch.Trim();
                if (string.IsNullOrWhiteSpace(commandText))
                    continue;

                using var command = connection.CreateCommand();
                command.Transaction = (SqlTransaction)transaction;
                command.CommandText = commandText;
                command.CommandTimeout = 600;
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            Console.WriteLine("Archivo .sql ejecutado correctamente ✅");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"❌ Error ejecutando el archivo .sql: {ex.Message}");
            throw;
        }
        finally
        {
            if (wasClosed)
                await connection.CloseAsync();
        }
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country
            {
                Name = "Colombia",
                States = [
                    new State()
                    {
                        Name = "Antioquia",
                        Cities = [
                            new City() { Name = "Medellín" },
                            new City() { Name = "Itagüí" },
                            new City() { Name = "Envigado" },
                            new City() { Name = "Bello" },
                            new City() { Name = "Rionegro" },
                        ]
                    },
                    new State()
                    {
                        Name = "Bogotá",
                        Cities = [
                            new City() { Name = "Usaquen" },
                            new City() { Name = "Champinero" },
                            new City() { Name = "Santa fe" },
                            new City() { Name = "Useme" },
                            new City() { Name = "Bosa" },
                        ]
                    },
                ]
            });
            _context.Countries.Add(new Country
            {
                Name = "Estados Unidos",
                States = [
                    new State()
                {
                    Name = "Florida",
                    Cities = [
                        new City() { Name = "Orlando" },
                        new City() { Name = "Miami" },
                        new City() { Name = "Tampa" },
                        new City() { Name = "Fort Lauderdale" },
                        new City() { Name = "Key West" },
                    ]
                },
                new State()
                    {
                        Name = "Texas",
                        Cities = [
                            new City() { Name = "Houston" },
                            new City() { Name = "San Antonio" },
                            new City() { Name = "Dallas" },
                            new City() { Name = "Austin" },
                            new City() { Name = "El Paso" },
                        ]
                    },
                ]
            });
        }
        await _context.SaveChangesAsync();
    }

    private async Task CheckEmployeesAsync()
    {
        if (!_context.Employees.Any())
        {
            _context.Employees.Add(new Employee { FirstName = "Juan", LastName = "Gomez", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 1500000M });
            _context.Employees.Add(new Employee { FirstName = "Juana", LastName = "Perez", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 2000000M });
            _context.Employees.Add(new Employee { FirstName = "Julian", LastName = "Lopez", IsActive = false, HireDate = DateTime.Now.AddYears(-5), Salary = 2500000M });
            _context.Employees.Add(new Employee { FirstName = "Ana Julia", LastName = "Martinez", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 1800000M });
            _context.Employees.Add(new Employee { FirstName = "Carlos", LastName = "Juarez", IsActive = true, HireDate = DateTime.Now.AddYears(-4), Salary = 1200000M });
            _context.Employees.Add(new Employee { FirstName = "Luisa", LastName = "San Juan", IsActive = true, HireDate = DateTime.Now.AddYears(-6), Salary = 2200000M });
            _context.Employees.Add(new Employee { FirstName = "Pedro", LastName = "Ramirez", IsActive = true, HireDate = DateTime.Now.AddYears(-7), Salary = 1400000M });
            _context.Employees.Add(new Employee { FirstName = "Miguel", LastName = "Santos", IsActive = false, HireDate = DateTime.Now.AddYears(-8), Salary = 1300000M });
            _context.Employees.Add(new Employee { FirstName = "Laura", LastName = "Diaz", IsActive = true, HireDate = DateTime.Now.AddYears(-1).AddMonths(-2), Salary = 1600000M });
            _context.Employees.Add(new Employee { FirstName = "Andres", LastName = "Quintero", IsActive = true, HireDate = DateTime.Now.AddMonths(-6), Salary = 1700000M });

            _context.Employees.Add(new Employee { FirstName = "Camila", LastName = "Suarez", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2100000M });
            _context.Employees.Add(new Employee { FirstName = "Felipe", LastName = "Ruiz", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2300000M });
            _context.Employees.Add(new Employee { FirstName = "Valentina", LastName = "Mora", IsActive = false, HireDate = DateTime.Now.AddYears(-5), Salary = 1800000M });
            _context.Employees.Add(new Employee { FirstName = "Daniel", LastName = "Lopez", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 1950000M });
            _context.Employees.Add(new Employee { FirstName = "Isabella", LastName = "Cardona", IsActive = true, HireDate = DateTime.Now.AddYears(-4), Salary = 2500000M });
            _context.Employees.Add(new Employee { FirstName = "Samuel", LastName = "Hernandez", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2850000M });
            _context.Employees.Add(new Employee { FirstName = "Lucia", LastName = "Ramirez", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2650000M });
            _context.Employees.Add(new Employee { FirstName = "Mateo", LastName = "Gonzalez", IsActive = false, HireDate = DateTime.Now.AddYears(-6), Salary = 1900000M });
            _context.Employees.Add(new Employee { FirstName = "Sara", LastName = "Ortega", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2400000M });
            _context.Employees.Add(new Employee { FirstName = "Tomas", LastName = "Rincon", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 3100000M });

            _context.Employees.Add(new Employee { FirstName = "Juliana", LastName = "Morales", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2750000M });
            _context.Employees.Add(new Employee { FirstName = "Sebastian", LastName = "Reyes", IsActive = false, HireDate = DateTime.Now.AddYears(-7), Salary = 1600000M });
            _context.Employees.Add(new Employee { FirstName = "Ana", LastName = "Vargas", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2000000M });
            _context.Employees.Add(new Employee { FirstName = "David", LastName = "Garcia", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 2300000M });
            _context.Employees.Add(new Employee { FirstName = "Mariana", LastName = "Castaño", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2600000M });
            _context.Employees.Add(new Employee { FirstName = "Simon", LastName = "Patiño", IsActive = false, HireDate = DateTime.Now.AddYears(-5), Salary = 1800000M });
            _context.Employees.Add(new Employee { FirstName = "Elena", LastName = "Salazar", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2150000M });
            _context.Employees.Add(new Employee { FirstName = "Gabriel", LastName = "Restrepo", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2450000M });
            _context.Employees.Add(new Employee { FirstName = "Natalia", LastName = "Cano", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 2700000M });
            _context.Employees.Add(new Employee { FirstName = "Emilio", LastName = "Lozano", IsActive = false, HireDate = DateTime.Now.AddYears(-8), Salary = 1500000M });

            _context.Employees.Add(new Employee { FirstName = "Daniela", LastName = "Peña", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 2900000M });
            _context.Employees.Add(new Employee { FirstName = "Martin", LastName = "Giraldo", IsActive = true, HireDate = DateTime.Now.AddYears(-4), Salary = 2600000M });
            _context.Employees.Add(new Employee { FirstName = "Claudia", LastName = "Rivera", IsActive = false, HireDate = DateTime.Now.AddYears(-6), Salary = 1700000M });
            _context.Employees.Add(new Employee { FirstName = "Leonardo", LastName = "Zuluaga", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2500000M });
            _context.Employees.Add(new Employee { FirstName = "Paula", LastName = "Bedoya", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 3100000M });
            _context.Employees.Add(new Employee { FirstName = "Ricardo", LastName = "Ospina", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2800000M });
            _context.Employees.Add(new Employee { FirstName = "Veronica", LastName = "Cordoba", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2700000M });
            _context.Employees.Add(new Employee { FirstName = "Oscar", LastName = "Perez", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 3000000M });
            _context.Employees.Add(new Employee { FirstName = "Tatiana", LastName = "Muñoz", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2650000M });
            _context.Employees.Add(new Employee { FirstName = "Pablo", LastName = "Cardenas", IsActive = true, HireDate = DateTime.Now.AddYears(-4), Salary = 2300000M });

            _context.Employees.Add(new Employee { FirstName = "Angela", LastName = "Mejia", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2550000M });
            _context.Employees.Add(new Employee { FirstName = "Cristian", LastName = "Torres", IsActive = false, HireDate = DateTime.Now.AddYears(-7), Salary = 1850000M });
            _context.Employees.Add(new Employee { FirstName = "Adriana", LastName = "Londoño", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2400000M });
            _context.Employees.Add(new Employee { FirstName = "Jorge", LastName = "Arango", IsActive = true, HireDate = DateTime.Now.AddYears(-4), Salary = 2750000M });
            _context.Employees.Add(new Employee { FirstName = "Sofia", LastName = "Guzman", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 2950000M });
            _context.Employees.Add(new Employee { FirstName = "Federico", LastName = "Marin", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 2600000M });
            _context.Employees.Add(new Employee { FirstName = "Liliana", LastName = "Vega", IsActive = false, HireDate = DateTime.Now.AddYears(-5), Salary = 1900000M });
            _context.Employees.Add(new Employee { FirstName = "Oscar", LastName = "Loaiza", IsActive = true, HireDate = DateTime.Now.AddYears(-6), Salary = 2100000M });
            _context.Employees.Add(new Employee { FirstName = "Nataly", LastName = "Rojas", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 2800000M });
            _context.Employees.Add(new Employee { FirstName = "Eduardo", LastName = "Benitez", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 3200000M });

            await _context.SaveChangesAsync();
        }
    }
}