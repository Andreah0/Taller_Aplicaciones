using System.ComponentModel;

namespace Company.Shared.Enums;

public enum UserType
{
    [Description("Administrador")]
    Admin,

    [Description("Usuario")]
    User
}