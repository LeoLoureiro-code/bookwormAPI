using System.ComponentModel.DataAnnotations;

namespace bookwormAPI.DTO
{
    public class UserDTO
    {

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;


        public string RefreshToken { get; set; } = null!;

    }
}
