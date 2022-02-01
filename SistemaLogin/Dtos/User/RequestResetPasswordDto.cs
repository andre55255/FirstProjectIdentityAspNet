using System.ComponentModel.DataAnnotations;

namespace SistemaLogin.Dtos.User
{
    public class RequestResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
