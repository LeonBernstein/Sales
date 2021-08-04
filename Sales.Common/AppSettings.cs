using System.ComponentModel.DataAnnotations;

namespace Sales.Common
{
    public class AppSettings
    {
        [Required]
        public string AppName { get; set; }
        [Required]
        public int OPTLength { get; set; }
        [Required]
        public int AuthTokenExpInHours { get; set; }
        [Required]
        public string AuthCookieName { get; set; }
        [Required]
        public string SecureKey { get; set; }
    }
}
