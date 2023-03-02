using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class SignInModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
