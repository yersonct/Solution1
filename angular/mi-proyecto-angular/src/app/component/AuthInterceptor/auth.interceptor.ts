import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpInterceptor,
  HttpHeaders, // Importa HttpHeaders
} from '@angular/common/http';
import { AuthService } from '../service/auth.service';
import { Observable } from 'rxjs'; // Importa Observable
import { HttpEvent } from '@angular/common/http'; // Importa HttpEvent

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authToken = this.authService.getToken();
    console.log('Token en el interceptor:', authToken); // Agrega este log

    if (authToken) {
      const authReq = req.clone({
        headers: new HttpHeaders({
          'Authorization': `Bearer ${authToken}`,
          // Puedes añadir otras cabeceras aquí si es necesario
        }),
      });
      console.log('Petición modificada con token:', authReq); // Agrega este log
      return next.handle(authReq);
    }

    console.log('Petición sin token:', req); // Agrega este log si no hay token
    return next.handle(req);
  }
}
