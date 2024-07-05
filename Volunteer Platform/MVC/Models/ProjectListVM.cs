using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class ProjectListVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Type")]
        public string TypeName { get; set; }

        [Display(Name = "Image Path")]
        public string? ImageFilePath { get; set; }
    }
}
