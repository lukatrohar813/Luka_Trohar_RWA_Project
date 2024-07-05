using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class ProjectDeleteVM
    {
        [DisplayName("Project Id")]
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [DisplayName("Type Id")]
        public int TypeId { get; set; }
        [DisplayName("Type")]
        public string TypeName { get; set; }
        [DisplayName("Image Id")]
        public int ImageId { get; set; }
        [DisplayName("Image")]
        public string ImageFilePath { get; set; }
        [DisplayName("Skills required")]
        public List<string> Skills { get; set; }
        [DisplayName("Users on project")]
        public List<string> Users { get; set; }
    }
}
