using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVC.Models;

public class TypeVM
{
	[Key] public int Id { get; set; }

	[Required]
	[StringLength(256, ErrorMessage = "Name length can't be more than 36.")]
	[DisplayName("Name")]
	public string Name { get; set; }
	[Required]
	[StringLength(512, ErrorMessage = "Description length can't be more than 256.")]
	[DisplayName("Description")]
	public string? Description { get; set; }
}