import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component'; // Crea este componente si no existe
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { FormComponent } from './components/form/form.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'Formularios', component: FormComponent },

];