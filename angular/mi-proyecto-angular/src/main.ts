// main.ts
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient, HTTP_INTERCEPTORS, withInterceptors } from '@angular/common/http'; // Importa HTTP_INTERCEPTORS
import { enableProdMode, importProvidersFrom } from '@angular/core'; // Importa importProvidersFrom
import { environment } from './app/component/environments/environment';
import { routes } from './app/app.routes';
import { AuthInterceptor } from './app/component/AuthInterceptor/auth.interceptor'; // Ajusta la ruta al interceptor
import { appConfig } from './app/app.config';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, appConfig)
.catch(err => console.error(err));