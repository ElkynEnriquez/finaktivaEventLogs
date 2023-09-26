import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ToolbarRoutingModule } from './toolbar-routing.module';
import { ToolbarComponent } from './toolbar.component';
import { MaterialModule } from 'projects/shared/src/public-api';


@NgModule({
  declarations: [
    ToolbarComponent
  ],
  imports: [
    CommonModule,
    ToolbarRoutingModule,
    MaterialModule, //agregado
  ],
  exports: [
    ToolbarComponent
  ]
})
export class ToolbarModule { }
