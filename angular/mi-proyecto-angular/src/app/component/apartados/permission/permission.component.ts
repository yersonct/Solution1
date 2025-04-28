import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IPermission } from '../../Interfaces/ipermission';
import { ApiPermissionService } from '../../service/api.permission.service'; // Aj
import { FormsModule } from '@angular/forms'; // Importa FormsModule si vas a agregar formularios

@Component({
  selector: 'app-permission',
  imports: [CommonModule, FormsModule], // Asegúrate de incluir FormsModule si lo necesitas
  templateUrl: './permission.component.html',
  styleUrl: './permission.component.css',
  standalone: true

})
export class PermissionComponent implements OnInit {
  Permissions: IPermission[] = [];
  errorMessage: string = ''; // Para manejar errores
  permissionSeleccionado: IPermission | null = null;
  modoEdicion: boolean = false;
  nuevoPermission: { name: string; active?: boolean } = { name: '' };
  registroPermissionMensaje: string = '';
  registroPermissionError: string = '';

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
        this.errorMessage = 'Error al cargar los Permisos desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerPermissionPorId(id: number): void {
    this.ApiPermissionService.getById(id).subscribe({
      next: (permission) => {
        this.permissionSeleccionado = { ...permission };
        this.modoEdicion = true;
        console.log('Permiso obtenido para edición:', this.permissionSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el Permiso:', error);
        this.errorMessage = 'Error al cargar los detalles del permiso para editar.';
        this.modoEdicion = false;
        this.permissionSeleccionado = null;
      }
    });
  }

  actualizarPermission(permission: IPermission): void {
    this.obtenerPermissionPorId(permission.id); // Asumiendo que tu interfaz IPermission tiene un 'id'
  }

  guardarPermission(): void {
    if (this.permissionSeleccionado) {
      this.ApiPermissionService.putData(this.permissionSeleccionado.id, this.permissionSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Permiso actualizado:', response);
            this.loadPermission();
            this.modoEdicion = false;
            this.permissionSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el Permiso:', error);
            this.errorMessage = 'Error al guardar los cambios del permiso.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.permissionSeleccionado = null;
  }

  eliminarPermission(permission: IPermission): void {
    if (confirm(`¿Estás seguro de que deseas eliminar el permiso "${permission.name}"?`)) {
      this.ApiPermissionService.deleteData(permission.id).subscribe({ // Asumiendo que tienes un 'id'
        next: (response) => {
          console.log('Permiso eliminado:', response);
          this.loadPermission();
        },
        error: (error) => {
          console.error('Error al eliminar el Permiso:', error);
          this.errorMessage = 'Error al eliminar el permiso.';
        }
      });
    }
  }

  registrarNuevoPermission(): void {
    this.registroPermissionMensaje = '';
    this.registroPermissionError = '';
    this.ApiPermissionService.postData(this.nuevoPermission).subscribe({
      next: (response) => {
        console.log('Permiso registrado:', response);
        this.registroPermissionMensaje = 'Permiso registrado exitosamente.';
        this.nuevoPermission = { name: '' };
        this.loadPermission();
      },
      error: (error) => {
        console.error('Error al registrar el Permiso:', error);
        this.registroPermissionError = 'Error al registrar el permiso.';
      }
    });
  }
}