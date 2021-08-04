
using System.Text.Json.Serialization;

namespace Sales.Common.Entities
{
    public class UserEntity
    {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public bool IsOTPRequired { get; set; }
        public string PhoneNumber { get; set; }
    }
}
