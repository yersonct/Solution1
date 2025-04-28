import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IFormRolPermission } from '../../Interfaces/iform-rol-permission';
import { ApiFormRolPermissionService } from '../../service/api.form-rol-permission.service'; // Ajusta la ruta a tu ApiService

@Component({
  selector: 'app-form-rol-permission',
  imports: [CommonModule],
  templateUrl: './form-rol-permission.component.html',
  styleUrl: './form-rol-permission.component.css',
  standalone: true

})
export class FormRolPermissionComponent implements OnInit  {
  FormRolPermissions: IFormRolPermission[] = [];
  errorMessage: string = ''; // Para manejar errores

  constructor(private ApiFormRolPermissionService: ApiFormRolPermissionService) { }

  ngOnInit(): void {
    this.loadFormRolPermission();
  }

  loadFormRolPermission(): void {
    this.ApiFormRolPermissionService.getData().subscribe({
      next: (data) => {
        this.FormRolPermissions = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Rol desde la API.';
        console.error('Error:', error);
      }
    });
  }
}
