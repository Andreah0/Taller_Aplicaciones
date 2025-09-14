using System.ComponentModel.DataAnnotations;

namespace Company.Shared.Entities;

public class Employee
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Apellido")]
    [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool IsActive { get; set; }

    [Display(Name = "Fecha")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public DateTime HireDate { get; set; }

    [Display(Name = "Salario")]
    [Range(1000000, double.MaxValue, ErrorMessage = "El salario debe ser mínimo $1,000,000.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public decimal Salary { get; set; }
}