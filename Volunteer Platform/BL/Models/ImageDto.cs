using System.ComponentModel.DataAnnotations;

namespace BL.Models;

public class ImageDto
{
    [Display(Name = "Image Id")]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Content Type")]
    public string ContentType { get; set; }
    [Required]
    [Display(Name = "File Name")]
    public string FileName { get; set; }
    [Required]
    [Display(Name = "File Path")]
    public string FilePath { get; set; }

}