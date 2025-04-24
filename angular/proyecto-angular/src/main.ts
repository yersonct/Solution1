import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter, Routes } from '@angular/router';

// Define tus rutas aquí o impórtalas desde un archivo separado (recomendado)
const routes: Routes = [
  // { path: '', component: HomeComponent },  <-- Ejemplo, define tus rutas reales
  // { path: 'login', component: LoginComponent },
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes)
  ]
}).catch(err => console.error(err));
