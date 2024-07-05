using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BL.Models;

public class TypeDto
{
    [Key]
    public int? Id { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only alphabetic characters.")]
    [StringLength(256, ErrorMessage = "Name length can't be more than 36.")]
    [DisplayName("Name")]
    public string Name { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only alphabetic characters.")]
    [StringLength(512, ErrorMessage = "Description length can't be more than 256.")]
    [DisplayName("Description")]
    public string? Description { get; set; }
}