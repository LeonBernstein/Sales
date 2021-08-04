using System.ComponentModel.DataAnnotations;

namespace Sales.WebApi.Models
{
    public class LoginUserModel
    {
        [Required]
        [MinLength(7)]
        public string UserId { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
