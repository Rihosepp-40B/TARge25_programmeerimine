using System.ComponentModel.DataAnnotations;

namespace TARge25_Shop.Models.Account
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmationPassword { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
