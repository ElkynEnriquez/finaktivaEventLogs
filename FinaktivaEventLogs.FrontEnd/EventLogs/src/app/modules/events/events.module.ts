import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventsComponent } from './events.component';
import { MaterialModule, SharedModule } from 'projects/shared/src/public-api';
import { EventsRoutingModule } from './events-routing.module';
import { TableEventComponent } from './table-event/table-event.component';
import { MatTableExporterModule } from 'mat-table-exporter';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DialogEventComponent } from './dialog-event/dialog-event.component';

@NgModule({
  imports: [
    CommonModule,
    EventsRoutingModule,
    MaterialModule,
    SharedModule,
    MatTableExporterModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  declarations: [
    EventsComponent,
    TableEventComponent,
    DialogEventComponent
  ]
})
export class EventsModule { }
