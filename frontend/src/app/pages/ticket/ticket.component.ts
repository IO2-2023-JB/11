import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Observable, Subscription, finalize, of, switchMap } from 'rxjs';
import { GetTicketDto } from 'src/app/core/models/tickets/get-ticket-dto';
import { TicketService } from 'src/app/core/services/ticket.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})
export class TicketComponent {
  tickets!: GetTicketDto[];
  subscriptions: Subscription[] = [];
  isProgressSpinnerVisible = false;


  constructor(
    private ticketService: TicketService,
    private messageService: MessageService,
    private router: Router) {
      this.getTickets();
    }

    goToSubmitter(submitterId: string) {
      this.router.navigate(['creator/' + submitterId]);
    }
  
    goToTarget(targetId: string) {
      // Implement your logic to navigate to the target's page
    }
  
    respondToTicket(ticketId: string) {
      // Implement your logic to respond to the ticket
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
