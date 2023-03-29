using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.DTO;

namespace YouTubeV2.Application.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal AccountBalance { get; set; }
        public int SubscriptionsCount { get; set; }
        public DateTime AccountCreationDate { get; init; }
        public User() { }

        public User(RegisterDto registerDto)
        {
            Email = registerDto.email; 
            UserName = registerDto.nickname;
            Name = registerDto.name;
            Surname = registerDto.surname;

            AccountBalance = 0;
            SubscriptionsCount = 0;
            AccountCreationDate = DateTime.Now;
        }
    }
}
