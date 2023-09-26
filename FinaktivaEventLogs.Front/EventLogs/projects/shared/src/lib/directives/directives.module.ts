import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UppercaseDirective } from './uppercase.directive';
import { DeleteSpacesInputsDirective } from './deleteSpacesInputs.directive';

@NgModule({
  declarations: [
    UppercaseDirective,
    DeleteSpacesInputsDirective
   ],
  imports: [
    CommonModule
  ],
  exports: [
    UppercaseDirective,
    DeleteSpacesInputsDirective
  ]
})
export class DirectivesModule { }
