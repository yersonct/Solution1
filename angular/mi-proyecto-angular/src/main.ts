// main.ts
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { enableProdMode } from '@angular/core';
import { environment } from './app/component/environments/environment'; // Asegúrate de que la ruta sea correcta
import { routes } from './app/app.routes'; // Importa directamente 'routes'

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes), // Usa el array 'routes' importado
    provideHttpClient()
  ]
}).catch(err => console.error(err));