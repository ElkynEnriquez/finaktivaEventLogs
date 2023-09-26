import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EnumService {
  constructor() {}
  public getEventTypes() {
    return [
      {
        value: 'Api',
        viewValue: 'Api',
      },
      {
        value: 'Formulario de eventos manuales',
        viewValue: 'Formulario',
      },
    ];
  }
}
