using System.ComponentModel.DataAnnotations;

namespace Sales.WebApi.Models
{
    public class LoginUserWithOtpModel : LoginUserModel
    {
        [Required]
        [StringLength(6)]
        public string OTP { get; set; }
    }
}
