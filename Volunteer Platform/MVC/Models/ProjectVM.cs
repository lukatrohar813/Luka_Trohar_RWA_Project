using BL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using DAL.Models;
using Type = DAL.Models.Type;

namespace MVC.Models
{
    public class ProjectVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public int? ImageId { get; set; }
        public IFormFile? Image { get; set; }
        public List<string>? Skills { get; set; }
        public List<string>? Users { get; set; }
        public string? ImageFilePath { get; set; }
        public bool?IsUserSignedToProject{ get; set; }



    }

}
