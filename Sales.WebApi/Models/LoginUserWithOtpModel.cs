using Sales.Common;
using System.ComponentModel.DataAnnotations;

namespace Sales.WebApi.Models
{
    public class LoginUserWithOtpModel : LoginUserModel
    {
        [Required]
        [StringLength(Globals.OPT_LENGTH)]
        public string OTP { get; set; }
    }
}
