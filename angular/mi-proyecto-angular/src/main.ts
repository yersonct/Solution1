// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router'; // Si usas rutas
import { provideHttpClient } from '@angular/common/http'; // Importa provideHttpClient

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter([]), // Configura tus rutas aquí si las tienes
    provideHttpClient() // Agrega provideHttpClient a los providers
  ]
})
  .catch((err) => console.error(err));