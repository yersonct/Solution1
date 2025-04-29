import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './component/login/login.component'; // Asegúrate de la ruta correcta
import { RegisterComponent } from './component/registro/registro.component'; // Asegúrate de la ruta correcta

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  // Otras rutas de tu aplicación
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Ruta por defecto
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }