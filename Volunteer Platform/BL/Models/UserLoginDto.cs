﻿using System.ComponentModel.DataAnnotations;


namespace BL.Models
{
    public class UserLoginDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
		[DataType(DataType.Password)]

		public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
       
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
