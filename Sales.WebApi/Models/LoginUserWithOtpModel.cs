using System.ComponentModel.DataAnnotations;

namespace Sales.WebApi.Models
{
    public class LoginUserWithOtpModel : LoginUserModel
    {
        [Required]
        [StringLength(6)] // If this value is changed, AppSettings should also be updated.
        public string OTP { get; set; }
    }
}
