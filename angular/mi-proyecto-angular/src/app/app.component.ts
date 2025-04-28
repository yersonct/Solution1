import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormComponent } from './component/apartados/form/form.component';
import { RolComponent } from './component/apartados/rol/rol.component';
import { PersonComponent } from './component/apartados/person/person.component';
import { UserComponent } from './component/apartados/user/user.component';
import { RolUserComponent } from './component/apartados/rol-user/rol-user.component';
import { FormRolPermissionComponent } from './component/apartados/form-rol-permission/form-rol-permission.component';
import { FormModuleComponent } from './component/apartados/form-module/form-module.component';
import { PermissionComponent } from './component/apartados/permission/permission.component';
import { ModuleComponent } from './component/apartados/module/module.component';
import { HttpClientModule } from '@angular/common/http';
import { MenuLateralComponent } from './component/menu-lateral/menu-lateral.component';
import { CommonModule } from '@angular/common'; // Importa CommonModule

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, // Asegúrate de que CommonModule esté aquí
    MenuLateralComponent,
    RouterOutlet,
    FormComponent,
    RolComponent,
    PersonComponent,
    UserComponent,
    RolUserComponent,
    FormRolPermissionComponent,
    FormModuleComponent,
    PermissionComponent,
    ModuleComponent,
    HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'registro';
  activeComponent: string | null = null;
  
  onComponentToShow(componentName: string) {
    this.activeComponent = componentName;
  }
}