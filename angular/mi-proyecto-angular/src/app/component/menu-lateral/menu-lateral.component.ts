import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from '../apartados/form/form.component';
import { RolComponent } from '../apartados/rol/rol.component';
import { PersonComponent } from '../apartados/person/person.component';
import { UserComponent } from '../apartados/user/user.component';
import { RolUserComponent } from '../apartados/rol-user/rol-user.component';
import { FormRolPermissionComponent } from '../apartados/form-rol-permission/form-rol-permission.component';
import { FormModuleComponent } from '../apartados/form-module/form-module.component';
import { PermissionComponent } from '../apartados/permission/permission.component';
import { ModuleComponent } from '../apartados/module/module.component';

interface MenuItem {
  label: string;
  expanded: boolean;
  subItems?: SubMenuItem[];
}

interface SubMenuItem {
  label: string;
  componentName: string;
}

@Component({
  selector: 'app-menu-lateral',
  standalone: true,
  imports: [
    CommonModule,
    FormComponent,
    RolComponent,
    PersonComponent,
    UserComponent,
    RolUserComponent,
    FormRolPermissionComponent,
    FormModuleComponent,
    PermissionComponent,
    ModuleComponent,
  ],
  templateUrl: './menu-lateral.component.html',
  styleUrls: ['./menu-lateral.component.css']
})
export class MenuLateralComponent {
  @Output() componentToShow = new EventEmitter<string>(); // 👈 Asegúrate de que sea <string>

  menuItems: MenuItem[] = [
    { label: 'form', expanded: false, subItems: [{ label: 'form', componentName: 'FormComponent' }] },
    { label: 'form-module', expanded: false, subItems: [{ label: 'form-module', componentName: 'FormModuleComponent' }] },
    { label: 'form-rol-permission', expanded: false, subItems: [{ label: 'form-rol-permission', componentName: 'FormRolPermissionComponent' }] },
    { label: 'module', expanded: false, subItems: [{ label: 'module', componentName: 'ModuleComponent' }] },
    { label: 'permission', expanded: false, subItems: [{ label: 'permission', componentName: 'PermissionComponent' }] },
    { label: 'person', expanded: false, subItems: [{ label: 'person', componentName: 'PersonComponent' }] },
    { label: 'rol', expanded: false, subItems: [{ label: 'rol', componentName: 'RolComponent' }] },
    { label: 'rol-user', expanded: false, subItems: [{ label: 'rol-user', componentName: 'RolUserComponent' }] },
    { label: 'user', expanded: false, subItems: [{ label: 'user', componentName: 'UserComponent' }] }
  ];

  activeComponent: string | null = null;

  toggleMenu(item: MenuItem) {
    item.expanded = !item.expanded;
    this.menuItems.forEach(otherItem => {
      if (otherItem !== item) {
        otherItem.expanded = false;
      }
    });
  }

  showComponent(componentName: string) {
    this.componentToShow.emit(componentName); // 👈 Asegúrate de que componentName sea un string
  }
}