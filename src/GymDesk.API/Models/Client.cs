using System.ComponentModel.DataAnnotations.Schema;

namespace GymDesk.API.Models;

[Table("clients")]
public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateOnly RegistrationDate { get; set; }
}