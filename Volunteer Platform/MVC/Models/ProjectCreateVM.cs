using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class ProjectCreateVM
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "The Name of the project is required.")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Description field is required.")]
    [Display(Name = "Description")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The Type field is required.")]
    [Display(Name = "Type")]
    public int? TypeId { get; set; }

    [Required(ErrorMessage = "The Start Date field is required.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "The End Date field is required.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Image")]
    public IFormFile? Image { get; set; }

    public int? ImageId { get; set; }

    [Display(Name = "Skills")]
    public List<string> Skills { get; set; }
    
}