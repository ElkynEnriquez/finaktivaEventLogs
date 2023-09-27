import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule, routes } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { DirectivesModule, MaterialModule, SharedModule } from 'projects/shared/src/public-api';
import { ToolbarModule } from './modules/toolbar/toolbar.module';
import { MainNavigationModule } from './modules/main-navigation/main-navigation.module';
import { EventsModule } from './modules/events/events.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    DirectivesModule,
    SharedModule,
    ToolbarModule,
    MainNavigationModule,
    EventsModule,
    RouterModule.forRoot(routes),
  ],
  exports: [
    MaterialModule,
    BrowserAnimationsModule,
  ],
  providers: [{provide: LOCALE_ID, useValue: 'es-CO'}],
  bootstrap: [AppComponent]
})
export class AppModule { }
