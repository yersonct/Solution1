import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { AuthInterceptor } from './component/AuthInterceptor/auth.interceptor';
import { ErrorInterceptor } from './component/AuthInterceptor/error.interceptor'; // Importa el ErrorInterceptor
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CheckExpirationInterceptor } from './component/AuthInterceptor/check-expiration.interceptor';
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(), // No uses withInterceptors aquí directamente
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: CheckExpirationInterceptor, multi: true }, // Registra el nuevo interceptor
  ],
};