using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class ProfileEditVM
{
    [Required]
    public int Id { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]

    [Display(Name = "First Name")]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First Name must contain only alphabetic characters.")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last Name must contain only alphabetic characters.")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9])\+?[0-9\s\-\(\)]*$", ErrorMessage = "Phone number must contain at least 9 digits and can only contain digits, spaces, '+', '-', and '()'")]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; }
    public int Role { get; set; }
    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}