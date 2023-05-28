import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketComponent } from './ticket.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';



@NgModule({
  declarations: [
    TicketComponent
  ],
  exports: [
    TicketComponent
  ],
  imports: [
    CommonModule,
    TableModule,
    ButtonModule
  ]
})
export class TicketModule { }
