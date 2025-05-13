import { inject, Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpInterceptor,
  HttpHeaders,
  HttpInterceptorFn, // Importa HttpHeaders
} from '@angular/common/http';
import { AuthService } from '../service/auth.service';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const authToken = authService.getToken();
  console.log('Token en el interceptor (const):', authToken);

  if (authToken) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`,
      },
    });
    console.log('Petición modificada con token (const):', authReq);
    return next(authReq);
  }

  console.log('Petición sin token (const):', req);
  return next(req);
};

