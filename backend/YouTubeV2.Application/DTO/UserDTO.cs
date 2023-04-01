namespace YouTubeV2.Application.DTO
{
    public record UserDTO(Guid id, string email, string nickname, string name, string surname, 
        decimal accountBalance, string userType, string avatarImage, int subscriptionsCount);
}

