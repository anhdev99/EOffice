using System;

namespace EOffice.WebAPI.Authorization
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
        public TimeSpan TokenRefreshStore { get; set; }
        public TimeSpan NormalTokenLife { get; set; }
        public TimeSpan PaygovTokenLife { get; set; }
        public TimeSpan OthersTokenLife { get; set; }
    }
}