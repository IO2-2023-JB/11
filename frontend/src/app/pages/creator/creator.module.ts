import { NgModule } from '@angular/core';
import { CreatorComponent } from './creator.component';
import { PanelModule } from 'primeng/panel';
import { TabViewModule } from 'primeng/tabview';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    CreatorComponent
  ],
  exports: [
    CreatorComponent
  ],
  imports: [
    PanelModule,
    TabViewModule,
    CommonModule
  ]
})
export class CreatorModule { }
