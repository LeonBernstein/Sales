using Sales.Common;
using Sales.Common.Entities;
using Sales.Common.Exceptions;
using Sales.Common.Interfaces.BL;
using Sales.Common.Interfaces.DAL;
using Sales.Common.Interfaces.Services;
using System.Threading.Tasks;

namespace Sales.BL
{
    public class UsersLogic : IUsersLogic
    {
        private const int OPT_EXPIRATION_IN_MINS = 20;

        private readonly IUsersEngine _usersEngine;
        private readonly IOtpService _otpService;
        private readonly ICacheService _cacheService;
        private readonly ISmsService _smsService;

        public UsersLogic(
            IUsersEngine usersEngine,
            IOtpService otpService,
            ICacheService cacheService,
            ISmsService smsService
         )
        {
            _usersEngine = usersEngine;
            _otpService = otpService;
            _cacheService = cacheService;
            _smsService = smsService;
        }

        public async Task<UserEntity> GetUserByIdAsync(string userId)
        {
            UserEntity user = await _usersEngine.GetUserByIdAsync(userId);
            if (!user.IsOTPRequired) return user;
            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                throw new AppException("\"UsersLogic\" cannot send OTP to an empty phone number.");
            }
            string otp = _otpService.GenerateOTP();
            if (string.IsNullOrWhiteSpace(otp))
            {
                throw new AppException("\"UsersLogic\" received an invalid one time password.");
            }
            var cacheOTPItem = CacheItemEntity.CreateOTPCacheItem(userId, otp, OPT_EXPIRATION_IN_MINS);
            _cacheService.UpsertItem(cacheOTPItem);
            string otpSmsMessage = GenerateOtpSmsMessage(otp);
            _smsService.SendSmsInBackground(user.PhoneNumber, otpSmsMessage);
            return user;
        }

        public async Task<bool> AreUsersDetailsValidAsync(string userId, string password, string otp = null)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password)) return false;
            UserEntity user = await _usersEngine.GetUserByIdAsync(userId);
            if (user == null
             || userId.ToLower() != user.UserId.ToLower()
             || password != user.Password
            )
            {
                return false;
            }
            if (!user.IsOTPRequired) return true;
            if (otp == null) return false;
            string cachedOtp = _cacheService.GetItem<string>(CacheRegions.OPT, userId);
            var isOtpValid = cachedOtp == otp;
            if (isOtpValid) _cacheService.RemoveItem(CacheRegions.OPT, userId);
            return isOtpValid;
        }

        public async Task<bool> IsUserExistsAsync(string userId)
        {
            return await _usersEngine.IsUserExistsAsync(userId);
        }

        private static string GenerateOtpSmsMessage(string otp)
        {
            return $"Sales App: Your security code is: {otp}. It expires in {OPT_EXPIRATION_IN_MINS} minutes.";
        }
    }
}
