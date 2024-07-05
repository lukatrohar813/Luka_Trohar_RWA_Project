using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BL.Models;

public class ProjectDto
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
    [DisplayName("Type")]
    public int TypeId { get; set; }
    [Required]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    [Required]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [DisplayName("Image")]
    public int? ImageId { get; set; }
    public string? ImageFilePath { get; set; }
    public string? TypeName { get; set; }
    [DisplayName("Skills")]
    public List<string>? Skills { get; set; } = new List<string>();
    [DisplayName("Users")]
    public List<string>? Users { get; set; } = new List<string>();
   





}