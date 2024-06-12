using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
