using BL.Models;

namespace MVC.Models
{
    public class ApplicationVM
    {
        public ApplicationDto application { get; set; }
        List<string> Users { get; set; }
        List<string> Projects { get; set; }
    }
}
