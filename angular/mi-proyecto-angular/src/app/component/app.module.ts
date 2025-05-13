import { BrowserModule } from '@angular/platform-browser';
import { NgModule, importProvidersFrom } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthInterceptor } from './AuthInterceptor/auth.interceptor';
import { provideRouter } from '@angular/router'; // Si tienes rutas definidas aquí o en otro lugar

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    CommonModule,
    // Si tienes rutas definidas en este módulo, descomenta:
    // RouterModule.forRoot([...tusRutas])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    // Otros servicios globales
    // Si usas provideRouter en app.config.ts, no lo necesitas aquí
  ],
  // No necesitas declarations ni bootstrap cuando AppComponent es standalone
})
export class AppModule { }