using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<Activity> Activites { get; set; }
}

public enum Role
{
    Admin,
    Trainer,
    Customer
}