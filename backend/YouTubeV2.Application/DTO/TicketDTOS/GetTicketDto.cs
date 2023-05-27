using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.DTO.TicketDTOS
{
    public record GetTicketDto(Guid SubmitterId, Guid TargetId, string Reason, GetTicketStatusDto Status, string Response)
    {
        public GetTicketDto(Ticket ticket) : this(ticket.Id, ticket.TargetId, ticket.Reason, new GetTicketStatusDto(ticket.Status), ticket.Response) { }
    }
}

