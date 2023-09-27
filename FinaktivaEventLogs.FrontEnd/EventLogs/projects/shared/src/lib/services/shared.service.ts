import { Injectable } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { BehaviorSubject, Observable, Subject, filter, tap } from 'rxjs';
import Swal from 'sweetalert2';
import { IJsonPatch } from '../interfaces/IJsonPatch';
import { IResponse } from '../interfaces/IResponse';
import { GlobalService } from './global.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
//se define endPoint del microFront
private url = "";

  constructor(
    private dialog : MatDialog,
    private globalService: GlobalService,
    private router: Router
  ) { }

  /**
   * Inicio
   * Consultas generales y observables
   */
  /**
   * Fin
   * Consultas generales y observables
   */

  /**
   * Alert de error con grafico y mensaje
   * @param error error en el mensaje
   */
  public errorConnectionDB (error: any) {
    if(error.title)
      error = error.title;
    let displayError = 'Error: '+ error ?? 'Falla al consultar base de datos';
    console.log("displayError", displayError)
    Swal.fire({
      title: 'Oppsss!, error',
      icon: 'error',
      html: ` <p>Por favor, intente de nuevo, recuerde revisar su conexión a internet y la información enviada.</p>
              <div class="row">
                <div class="col-xs-8 col-xs-offset-2">

                <br>
                </div>
              </div>
              <h3 class="light-red-lilisoft-text">`+ displayError +`</h3>
              <h5>Si el problema persiste, informe al correo <a href="mailto:dev@finaktiva.com" target="_blank">dev@finaktiva.com</a></h5>`,
      confirmButtonColor: '#085092',
      allowOutsideClick: false,
    });
  }
}
