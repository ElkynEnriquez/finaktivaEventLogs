import { Routes, RouterModule } from '@angular/router';
import { EventsComponent } from './events.component';
import { NgModule } from '@angular/core';
import { TableEventComponent } from './table-event/table-event.component';

const routes: Routes = [
  { path: '', component: EventsComponent },
  { path: 'list', component: TableEventComponent },
  { path: 'id/:id', component: EventsComponent },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EventsRoutingModule {}
