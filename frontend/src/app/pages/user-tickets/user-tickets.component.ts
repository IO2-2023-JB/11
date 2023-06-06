import { Component } from '@angular/core';
import { Observable, Subscription, finalize, of, switchMap } from 'rxjs';
import { GetTicketDto } from 'src/app/core/models/tickets/get-ticket-dto';
import { TicketService } from 'src/app/core/services/ticket.service';

@Component({
  selector: 'app-user-tickets',
  templateUrl: './user-tickets.component.html',
  styleUrls: ['./user-tickets.component.scss']
})
export class UserTicketsComponent {
  tickets!: GetTicketDto[];
  subscriptions: Subscription[] = [];
  isProgressSpinnerVisible = false;

  constructor(private ticketService: TicketService) {
    this.getTickets();
  }

  getTickets() {
    this.subscriptions.push(
      this.ticketService.getTicketList().subscribe(tickets => {
        this.tickets = tickets;
      }));
  }
  
  doWithLoading(observable$: Observable<any>): Observable<any> {
    return of(this.isProgressSpinnerVisible = true).pipe(
      switchMap(() => observable$),
      finalize(() => this.isProgressSpinnerVisible = false)
    );
  }
}
