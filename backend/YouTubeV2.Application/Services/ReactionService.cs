﻿using Microsoft.EntityFrameworkCore;
using YouTubeV2.Application.Enums;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly YTContext _context;

        public ReactionService(YTContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateReactionAsync(ReactionType reactionType, Video video, User user, CancellationToken cancellationToken = default)
        {
            Reaction? reaction = await _context
                .Reactions
                .FirstOrDefaultAsync(reaction => reaction.Video.Id == video.Id && reaction.User.Id == user.Id);

            if (reaction is null) await AddReactionAsync(reactionType, video, user, cancellationToken);
            else await UpdateReactionAsync(reaction, reactionType, cancellationToken);
        }

        private async Task AddReactionAsync(ReactionType reactionType, Video video, User user, CancellationToken cancellationToken)
        {
            if (reactionType == ReactionType.None) return;
            await _context.Reactions.AddAsync(new Reaction(reactionType, user, video), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateReactionAsync(Reaction reaction,ReactionType reactionType, CancellationToken cancellationToken)
        {
            if (reactionType == ReactionType.None) _context.Reactions.Remove(reaction);
            else reaction.ReactionType = reactionType;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
