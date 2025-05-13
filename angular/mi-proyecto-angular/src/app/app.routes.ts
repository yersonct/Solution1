    import { RouterModule, Routes } from '@angular/router';
    import { LoginComponent } from './component/login/login.component';
    import { RegisterComponent } from './component/registro/registro.component';
    import { MenuLateralComponent } from './component/menu-lateral/menu-lateral.component';
    import { AuthGuard } from './component/guards/auth.guard';
    import { FormComponent } from './component/apartados/form/form.component';
    import { RolComponent } from './component/apartados/rol/rol.component';
    import { PersonComponent } from './component/apartados/person/person.component';
    import { UserComponent } from './component/apartados/user/user.component';
    import { RolUserComponent } from './component/apartados/rol-user/rol-user.component';
    import { FormRolPermissionComponent } from './component/apartados/form-rol-permission/form-rol-permission.component';
    import { FormModuleComponent } from './component/apartados/form-module/form-module.component';
    import { PermissionComponent } from './component/apartados/permission/permission.component';
    import { ModuleComponent } from './component/apartados/module/module.component';
import { NgModule } from '@angular/core';

  export const routes: Routes = [
      { path: 'login', component: LoginComponent },
      { path: 'registro', component: RegisterComponent },
      {
        path: 'app',
        component: MenuLateralComponent,
        canActivate: [AuthGuard],
        children: [
          { path: 'form', component: FormComponent },
          { path: 'rol', component: RolComponent },
          { path: 'person', component: PersonComponent },
          { path: 'user', component: UserComponent },
          { path: 'rol-user', component: RolUserComponent },
          { path: 'form-rol-permission', component: FormRolPermissionComponent },
          { path: 'form-module', component: FormModuleComponent },
          { path: 'permission', component: PermissionComponent },
          { path: 'module', component: ModuleComponent },
          { path: '', redirectTo: 'form', pathMatch: 'full' },
        ],
      },
      { path: '', redirectTo: '/login', pathMatch: 'full' },
      { path: '**', redirectTo: '/login' },
    ];

  @NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
