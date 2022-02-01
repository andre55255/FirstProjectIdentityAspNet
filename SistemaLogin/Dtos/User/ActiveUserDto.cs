using System.ComponentModel.DataAnnotations;

namespace SistemaLogin.Dtos.User
{
    public class ActiveUserDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "CodeActive is required")]
        public string CodeActive { get; set; }
    }
}
