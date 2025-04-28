import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; // Importa HttpClientModule

import { AppComponent } from '../app.component';
import { CommonModule } from '@angular/common';
import { FormComponent } from './apartados/form/form.component';
import { RolComponent } from './apartados/rol/rol.component';
import { PersonComponent } from './apartados/person/person.component';
import { UserComponent } from './apartados/user/user.component';
import { RolUserComponent } from './apartados/rol-user/rol-user.component';
import { FormRolPermissionComponent } from './apartados/form-rol-permission/form-rol-permission.component';
import { FormModuleComponent } from './apartados/form-module/form-module.component';
import { PermissionComponent } from './apartados/permission/permission.component';
import { ModuleComponent } from './apartados/module/module.component';
import { MenuLateralComponent } from './menu-lateral/menu-lateral.component';
// ... otros imports

@NgModule({
  declarations: [
    AppComponent,
    MenuLateralComponent,
    FormComponent,
    RolComponent,
    PersonComponent,
    UserComponent,
    RolUserComponent,
    FormRolPermissionComponent,
    FormModuleComponent,
    PermissionComponent,
    ModuleComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule, // Agrega HttpClientModule a los imports
    CommonModule
  ],
  exports:[
    MenuLateralComponent,
    FormComponent,
    RolComponent,
    PersonComponent,
    UserComponent,
    RolUserComponent,
    FormRolPermissionComponent,
    FormModuleComponent,
    PermissionComponent,
    ModuleComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }