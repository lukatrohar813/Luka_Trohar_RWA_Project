using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class ProjectDetailsVM
{

    public int Id { get; set; }

    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; }

    [Display(Name = "Type")]
    public string TypeName { get; set; }

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Image File Path")]
    public string ImageFilePath { get; set; }

    [Display(Name = "Skills")]
    public List<string> Skills { get; set; }

    [Display(Name = "Users")]
    public List<string> Users { get; set; }

    [Display(Name = "Is User Signed to Project")]
    public bool IsUserSignedToProject { get; set; }

}