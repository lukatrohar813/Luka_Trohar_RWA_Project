using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVC.Models
{
    public class ListAdminVM
    {
        [Display(Name = "Project Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }

        [DisplayName("Type")]
        public string TypeName { get; set; }

        [DisplayName("Image Path")]
        public string? ImageFilePath { get; set; }

        [DisplayName("Skills")]
        public List<string>? Skills { get; set; }

        [DisplayName("Users")]
        public List<string>? Users { get; set; }
    }
}
