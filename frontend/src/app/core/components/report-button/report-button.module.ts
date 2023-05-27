import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { ReportButtonComponent } from './report-button.component';



@NgModule({
  declarations: [
    ReportButtonComponent
  ],
  exports: [
    ReportButtonComponent
  ],
  imports: [
    CommonModule,
    DialogModule
  ]
})
export class ReportButtonModule { }
