using System.ComponentModel.DataAnnotations;

namespace TARge25_Shop.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;

        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match.")] // kui seda ei ole, siis muudab parooli olenemata confirmationist
        public string ConfirmationPassword { get; set; } = string.Empty;
    }
}
