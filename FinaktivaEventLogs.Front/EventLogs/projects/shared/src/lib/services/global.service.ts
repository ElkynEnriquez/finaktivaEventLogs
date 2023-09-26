import { Observable } from 'rxjs';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { EnumService } from './enum.service';

@Injectable({
  providedIn: 'root',
})
export class GlobalService {
  @Output() update: EventEmitter<any> = new EventEmitter();

  public data: any = null;

  //private url = environment.url + environment.port;

  constructor(private http: HttpClient,
    private enumService: EnumService ) {}

  // -------------------------- Métodos HTTP globales -------------------------------- //
  /**
   * Método encargado de obtener la Data desde la base de datos, con conexión en BACK-END
   *
   * @param {string} api Variable complementaria para el EndPoint
   * @return Retorna la respuesta obtenida por el Back
   * @memberof GlobalService
   */
  getData(url: string, api: string): Observable<any> {
    return this.http.get(url + api);
  }

  /**
   * Método encargado de obtener la Data desde la base de datos, con conexión en BACK-END
   *
   * @param {string} api Variable complementaria para el EndPoint
   * @return Retorna la respuesta obtenida por el Back
   * @memberof GlobalService
   */
  getDataWithParams(
    url: string,
    api: string,
    parameters: any
  ): Observable<any> {
    // variable para convertir parametros a HttpParams
    let _httpParams = new HttpParams();
    for (const param in parameters) {
      // Si el parametro contiene un valor diferente a undefined se añade
      if (
        parameters.hasOwnProperty(param) &&
        parameters[param] !== undefined &&
        parameters[param] !== null
      ) {
        _httpParams = _httpParams.append(param, parameters[param]);
      }
    }
    return this.http.get(url + api, { params: _httpParams });
  }
  /**
   * Método encargado de traer la data desde la base de datos de un objeto en especifico, con conexión en BACK-END
   *
   * @param port Variable donde se recibe el puerto de conexión al back
   * @param api Variable complementaria para el EndPoint
   * @param id Id del objeto a consultar
   * @return Retorna la respuesta obtenida por el Back
   * @memberof GlobalServiceService
   */
  getById(url: string, api: string, id: string): Observable<any> {
    return this.http.get(url + api + id);
  }
  /**
   * Método encargado de enviar la data a la base de datos, con conexión en BACK-END
   *
   * @param port Variable donde se recibe el puerto de conexión al back
   * @param api Variable complementaria para el EndPoint
   * @param data Json con los datos enviado al back
   * @return Retorna la respuesta obtenida por el Back
   * @memberof GlobalServiceService
   */
  postData(url: string, api: string, data: any): Observable<any> {
    return this.http.post(url + api, data);
  }

  /**
   * Método encargado de editar la data y enviarla a la DB por patch
   *
   * @param url Variable donde se recibe la url de conexión al back
   * @param api Variable complementaria para el EndPoint
   * @param id Id del objeto a actualizar
   * @param data Variable en donde se envia la Data a insertar en la DB
   * @return Retorna la respuesta obtenida por el Back
   * @memberof GlobalServiceService
   */
  patchData(url: string, api: string, id: string, data: any) {
    return this.http.patch(`${url}${api}/${id}`,data)
  }

  /**
   * Método encargado de editar la data y enviarla a la DB
   *
   * @param port Variable donde se recibe el puerto de conexión al back
   * @param api Variable complementaria para el EndPoint
   * @param data Variable en donde se envia la Data a insertar en la DB
   * @param id Id del objeto a actualizar
   * @returns Retorna la data al Back
   */
  putData(url: string, api: string, data: any): Observable<any> {
    return this.http.put(url + api, data);
  }

  /**
   * Método encargado de eliminar la Data seleccionada
   * Recordar enviar el ID o el parametro necesario por el Back en la variable API
   *
   * @param port Variable donde se recibe el puerto de conexión al back
   * @param api Variable complementaria para el EndPoint
   * @param id Id del objeto a eliminar
   * @returns Retorna la data al Back
   */
  deleteData(url: string, api: string, id: any): Observable<any> {
    return this.http.delete(url + api + id);
  }
}
