using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BL.Models;

public class ProjectIncludeDto
{
	[Display(Name = "Project Id")]
	public int? Id { get; set; }
	[Required]
	[Display(Name = "Name")]
	public string Name { get; set; }
	[Required]
	[Display(Name = "Description")]
	public string Description { get; set; }
	[Required]
	[DisplayName("Type Id")]
	public int TypeId { get; set; }
	[Required]
	[DataType(DataType.Date)]
	[Display(Name = "Start Date")]
	public DateTime StartDate { get; set; }
	[Required]
	[DataType(DataType.Date)]
	[Display(Name = "End Date")]
	public DateTime EndDate { get; set; }
	[Required]
	[DisplayName("Image")]
	public int? ImageId { get; set; }
	[DisplayName("Image Path")]
	public string? ImageFilePath { get; set; }
    [DisplayName("Type")]
    public string? TypeName { get; set; }
	[DisplayName("Skills")]
	public List<string> Skills { get; set; } = new List<string>();
	[DisplayName("Users")]
	public List<string>? Users { get; set; } = new List<string>();

}