#nullable disable


namespace DAL.Models;

public partial class Type
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}