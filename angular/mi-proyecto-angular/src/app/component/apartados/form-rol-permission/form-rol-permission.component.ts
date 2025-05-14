import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IFormRolPermission } from '../../Interfaces/iform-rol-permission';
import { ApiFormRolPermissionService } from '../../service/api.form-rol-permission.service'; // Ajusta la ruta a tu ApiService
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-rol-permission',
  imports: [CommonModule,FormsModule], // Asegúrate de incluir FormsModule si lo necesitas
  templateUrl: './form-rol-permission.component.html',
  styleUrl: './form-rol-permission.component.css',
  standalone: true

})
export class FormRolPermissionComponent implements OnInit {
  FormRolPermissions: IFormRolPermission[] = [];
  errorMessage: string = '';
  formRolPermissionSeleccionado: IFormRolPermission | null = null;
  modoEdicion: boolean = false;
  nuevoFormRolPermission: { formName: string; rolName: string; permissionName: string; active?: boolean } = { formName: '', rolName: '', permissionName: '' };
  registroFormRolPermissionMensaje: string = '';
  registroFormRolPermissionError: string = '';

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
        this.errorMessage = 'Error al cargar los Permisos por Formulario y Rol desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerFormRolPermissionPorId(id: number): void {
    this.ApiFormRolPermissionService.getById(id).subscribe({
      next: (formRolPermission) => {
        this.formRolPermissionSeleccionado = { ...formRolPermission };
        this.modoEdicion = true;
        console.log('Permiso por Formulario y Rol obtenido para edición:', this.formRolPermissionSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el Permiso por Formulario y Rol:', error);
        this.errorMessage = 'Error al cargar los detalles para editar.';
        this.modoEdicion = false;
        this.formRolPermissionSeleccionado = null;
      }
    });
  }

  actualizarFormRolPermission(formRolPermission: IFormRolPermission): void {
    this.obtenerFormRolPermissionPorId(formRolPermission.id); // Asumiendo que tu interfaz IFormRolPermission tiene un 'id'
  }

  guardarFormRolPermission(): void {
    if (this.formRolPermissionSeleccionado) {
      this.ApiFormRolPermissionService.putData(this.formRolPermissionSeleccionado.id, this.formRolPermissionSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Permiso por Formulario y Rol actualizado:', response);
            this.loadFormRolPermission();
            this.modoEdicion = false;
            this.formRolPermissionSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el Permiso por Formulario y Rol:', error);
            this.errorMessage = 'Error al guardar los cambios.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.formRolPermissionSeleccionado = null;
  }

  eliminarFormRolPermission(formRolPermission: IFormRolPermission): void {
    if (confirm(`¿Estás seguro de que deseas eliminar el permiso para el formulario "${formRolPermission.formName}" y el rol "${formRolPermission.rolName}"?`)) {
      this.ApiFormRolPermissionService.deleteData(formRolPermission.id).subscribe({ // Asumiendo que tienes un 'id'
        next: (response) => {
          console.log('Permiso eliminado:', response);
          this.loadFormRolPermission();
        },
        error: (error) => {
          console.error('Error al eliminar el permiso:', error);
          this.errorMessage = 'Error al eliminar el permiso.';
        }
      });
    }
  }

  registrarNuevoFormRolPermission(): void {
    this.registroFormRolPermissionMensaje = '';
    this.registroFormRolPermissionError = '';
    this.ApiFormRolPermissionService.postData(this.nuevoFormRolPermission).subscribe({
      next: (response) => {
        console.log('Permiso registrado:', response);
        this.registroFormRolPermissionMensaje = 'Permiso registrado exitosamente.';
        this.nuevoFormRolPermission = { formName: '', rolName: '', permissionName: '' };
        this.loadFormRolPermission();
      },
      error: (error) => {
        console.error('Error al registrar el permiso:', error);
        this.registroFormRolPermissionError = 'Error al registrar el permiso.';
      }
    });
  }
}