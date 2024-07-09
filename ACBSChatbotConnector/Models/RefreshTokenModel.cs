using System;

namespace UserService.Models
{
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }  
    }
}
