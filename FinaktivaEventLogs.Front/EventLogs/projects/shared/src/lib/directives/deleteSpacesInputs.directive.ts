import { Directive, ElementRef, HostListener, OnDestroy, OnInit } from '@angular/core';
import { NgControl } from '@angular/forms';
import { Subject, distinctUntilChanged, takeUntil } from 'rxjs';

@Directive({
  selector: '[appDeleteSpacesInputs]'
})
export class DeleteSpacesInputsDirective implements OnInit, OnDestroy {
  destroy$ = new Subject();
  constructor(private control: NgControl) {}

  ngOnInit() {
    if (this.control.valueChanges) {
      this.control.valueChanges
        .pipe(takeUntil(this.destroy$), distinctUntilChanged())
        .subscribe((x) => {
          if (this.control && this.control.control) {
            this.control.control.setValue(x.trim());
          }
        });
    }
  }

  ngOnDestroy() {
    this.destroy$.unsubscribe();
  }
}
