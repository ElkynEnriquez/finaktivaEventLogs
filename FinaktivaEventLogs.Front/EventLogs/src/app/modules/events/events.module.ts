import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventsComponent } from './events.component';
import { MaterialModule, SharedModule } from 'projects/shared/src/public-api';
import { EventsRoutingModule } from './events-routing.module';
import { TableEventComponent } from './table-event/table-event.component';

@NgModule({
  imports: [
    CommonModule,
    EventsRoutingModule,
    MaterialModule,
    SharedModule,
  ],
  declarations: [
    EventsComponent,
    TableEventComponent
  ]
})
export class EventsModule { }
