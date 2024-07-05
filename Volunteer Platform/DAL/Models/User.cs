#nullable disable


namespace DAL.Models;

public partial class User
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PwdHash { get; set; }

    public string PwdSalt { get; set; }

    public string Phone { get; set; }

    public int Role { get; set; }

    public string SecurityToken { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}