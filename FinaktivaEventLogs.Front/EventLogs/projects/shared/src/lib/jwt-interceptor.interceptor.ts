import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import Swal from 'sweetalert2';

@Injectable()
export class JwtInterceptorInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const token: string | null = localStorage.getItem('token');
    let req = request;

    if(token) {
      req = request.clone({
        setHeaders: {
          authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req).pipe(
      catchError((error) => {
        let errorMessage = '';
        if (error instanceof ErrorEvent) {
          console.log(error);
          // client-side error
          errorMessage = `${error.error}`;
        } else {
          // backend error
          errorMessage = `${error.error}`;
          console.log(error);
          if (error.statusText === 'Unauthorized') {
            console.log('ERROR 401');
            Swal.fire(
              'Oops!',
              'Su sesión a caducado, por favor vuelva a iniciar sesión',
              'info'
            );
            alert('Su sesión a caducado, por favor vuelva a iniciar sesión');
            setTimeout(() => {
              console.log("LogOut");
              //logOut();
            }, 500);
          }
        }
        // aquí podrías agregar código que muestre el error en alguna parte fija de la pantalla.
        return throwError(errorMessage);
      })
    );
  }
}
