import { GetTicketStatusDto } from "./get-ticket-status-dto";

export interface GetTicketDto {
    submitterId: string;
    targetId: string;
    reason: string;
    status: GetTicketStatusDto;
    response: string;
  }