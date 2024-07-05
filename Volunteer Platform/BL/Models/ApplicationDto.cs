using System.ComponentModel.DataAnnotations;

namespace BL.Models;

public class ApplicationDto
{
    [Display(Name = "Application Id")]
    public int? Id { get; set; }
    [Required]
    [Display(Name = "User Id")]
    public int UserId { get; set; }
    [Required]
    [Display(Name = "Project Id")]
    public int ProjectId { get; set; }
    [Required]
    [Display(Name = "Status")]
    public string Status { get; set; }
    [Required]
    [Display(Name = "Project Name")]
    public string ProjectName { get; set; }
    [Required]
    [Display(Name = "Username")]
    public string UserUsername { get; set; }
  
    [Display(Name = "Application created")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
	public DateTime CreatedAt { get; set; }

}