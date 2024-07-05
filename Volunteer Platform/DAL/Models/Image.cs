#nullable disable


namespace DAL.Models;

public partial class Image
{
    public int Id { get; set; }

    public string ContentType { get; set; }

    public string FileName { get; set; }

    public string FilePath { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}