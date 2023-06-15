import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserTicketsComponent } from './user-tickets.component';
import { TableModule } from 'primeng/table';



@NgModule({
  declarations: [
    UserTicketsComponent
  ],
  exports: [
    UserTicketsComponent
  ],
  imports: [
    CommonModule,
    TableModule
  ]
})
export class UserTicketsModule { }
