﻿using YouTubeV2.Application.DTO;

namespace YouTubeV2.Application.Services
{
    public interface ISubscriptionService
    {
        Task<UserSubscriptionListDTO> GetSubscriptionsAsync(Guid Id, CancellationToken cancellationToken);
        Task<int> GetSubscriptionCount(Guid id, CancellationToken cancellationToken);
        Task PostSubscriptionsAsync(Guid id, string jwtToken, CancellationToken cancellationToken);
        Task DeleteSubscriptionsAsync(Guid id, string jwtToken, CancellationToken cancellationToken);
    }
}
