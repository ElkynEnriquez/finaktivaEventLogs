import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainNavigationRoutingModule } from './main-navigation-routing.module';
import { MainNavigationComponent } from './main-navigation.component';
import { MaterialModule } from 'projects/shared/src/public-api';


@NgModule({
  declarations: [
    MainNavigationComponent
  ],
  imports: [
    CommonModule,
    MainNavigationRoutingModule,
    MaterialModule
  ],
  exports: [
    MainNavigationComponent
  ]
})
export class MainNavigationModule { }
