import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../service/auth.service';

interface MenuItem {
  label: string;
  expanded: boolean;
  subItems?: SubMenuItem[];
}

interface SubMenuItem {
  label: string;
}

@Component({
  selector: 'app-menu-lateral',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    RouterOutlet, // Importa RouterOutlet aquí
  ],
  templateUrl: './menu-lateral.component.html',
  styleUrls: ['./menu-lateral.component.css']
})
export class MenuLateralComponent {
  menuItems: MenuItem[] = [
    { label: 'form', expanded: false, subItems: [{ label: 'Form' }] },
    { label: 'form-module', expanded: false, subItems: [{ label: 'FormModule' }] },
    { label: 'form-rol-permission', expanded: false, subItems: [{ label: 'FormRolPermission' }] },
    { label: 'module', expanded: false, subItems: [{ label: 'Module' }] },
    { label: 'permission', expanded: false, subItems: [{ label: 'Permission' }] },
    { label: 'person', expanded: false, subItems: [{ label: 'Person' }] },
    { label: 'rol', expanded: false, subItems: [{ label: 'Rol' }] },
    { label: 'rol-user', expanded: false, subItems: [{ label: 'RolUser' }] },
    { label: 'user', expanded: false, subItems: [{ label: 'User' }] }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  toggleMenu(item: MenuItem) {
    item.expanded = !item.expanded;
    this.menuItems.forEach(otherItem => {
      if (otherItem !== item) {
        otherItem.expanded = false;
      }
    });
  }

  logout() {
    this.authService.removeToken();
    this.router.navigate(['/login']);
  }
}