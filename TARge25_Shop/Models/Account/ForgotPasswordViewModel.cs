using System.ComponentModel.DataAnnotations;

namespace TARge25_Shop.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Enter your Email here")]
        public string Email { get; set; } = string.Empty;
    }
}
