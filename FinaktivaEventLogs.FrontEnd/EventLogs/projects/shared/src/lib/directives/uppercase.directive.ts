import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appUppercase]',
  providers: [],
})
export class UppercaseDirective {
  lastValue: string = '';

  constructor(public ref: ElementRef) {}

  @HostListener('input', ['$event']) onInput($event: any): any {
    const start = $event.target.selectionStart;
    const end = $event.target.selectionEnd;
    $event.target.value = $event.target.value.toUpperCase();
    $event.target.setSelectionRange(start, end);
    $event.preventDefault();

    if (
      !this.lastValue ||
      (this.lastValue &&
        $event.target.value.length > 0 &&
        this.lastValue !== $event.target.value)
    ) {
      this.lastValue = this.ref.nativeElement.value = $event.target.value;
      // Propagation
      const evt = new Event('input', {"bubbles":false, "cancelable":true});
      $event.target.dispatchEvent(evt);
    }
  }
  @HostListener('blur', ['$event']) onBlur($event: any): any {
    const start = $event.target.selectionStart;
    const end = $event.target.selectionEnd;
    $event.target.value = $event.target.value.trim();
    $event.target.setSelectionRange(start, end);
    $event.preventDefault();
  }
}
