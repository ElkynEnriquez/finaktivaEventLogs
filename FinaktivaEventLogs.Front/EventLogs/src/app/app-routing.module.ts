import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
  {
    title: 'Event Logs, for Finaktiva',
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
  },
  {
    title: 'Events | Finaktiva',
    path: 'events',
    loadChildren: () =>
      import('./modules/events/events.module').then(
        (m) => m.EventsModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
