using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeV2.Application.DTO
{
    public record VideoMetadataDto(string id,
      string title,
      string description,
      string thumbnail,
      string authorId,
      string authorNickname,
      int viewCount,
      List<string> tags,
      string visibility,
      string processingProgress,
      DateTime uploadDate,
      DateTime editDate,
      string duration);
}
