using BL.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class SkillVM
    {
        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only alphabetic characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Description must contain only alphabetic characters.")]
        [Display(Name = "Description")]
        public string? Description {get; set;}
    }
}
