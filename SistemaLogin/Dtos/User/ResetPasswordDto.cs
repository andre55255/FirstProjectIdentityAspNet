using System.ComponentModel.DataAnnotations;

namespace SistemaLogin.Dtos.User
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "RePassword is required")]
        [Compare("Password", ErrorMessage = "Passwords differents")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "TokenReset is required")]
        public string TokenReset { get; set; }
    }
}
