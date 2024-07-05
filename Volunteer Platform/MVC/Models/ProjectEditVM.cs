using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class ProjectEditVM
{
    
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public int TypeId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public int? ImageId { get; set; }
  
        public IFormFile? Image { get; set; }

        public string? ImageFilePath { get; set; }
        public List<string> Skills { get; set; }
    
}