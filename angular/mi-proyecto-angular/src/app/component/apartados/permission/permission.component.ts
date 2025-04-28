import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IPermission } from '../../Interfaces/ipermission';
import { ApiPermissionService } from '../../service/api.permission.service'; // Aj

@Component({
  selector: 'app-permission',
  imports: [CommonModule],
  templateUrl: './permission.component.html',
  styleUrl: './permission.component.css',
  standalone: true

})
export class PermissionComponent implements OnInit {
  Permissions: IPermission[] = [];
  errorMessage: string = ''; // Para manejar errores

  constructor(private ApiPermissionService: ApiPermissionService) { }

  ngOnInit(): void {
    this.loadPermission();
  }

  loadPermission(): void {
    this.ApiPermissionService.getData().subscribe({
      next: (data) => {
        this.Permissions = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Permission desde la API.';
        console.error('Error:', error);
      }
    });
  }
}
