using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeV2.Application.DTO
{
    public record UserDto(string id, string email, string nickname, string name, string surname, double accountBalance, string userType, string avatarImage, int subscriptionsCount);
}
