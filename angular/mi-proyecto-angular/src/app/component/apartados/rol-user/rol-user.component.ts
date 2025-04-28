import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IRolUser } from '../../Interfaces/irol-user';
import { ApiRolUserService } from '../../service/api.rol-user.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-rol-user',
  imports: [CommonModule, FormsModule],
  templateUrl: './rol-user.component.html',
  styleUrl: './rol-user.component.css',
  standalone: true

})
export class RolUserComponent implements OnInit {
  RolUsers: IRolUser[] = [];
  errorMessage: string = ''; // Para manejar errores
  rolUserSeleccionado: IRolUser | null = null;
  modoEdicion: boolean = false;
  nuevoRolUser: { rolName: string; userName: string; active: boolean } = { rolName: '', userName: '', active: false };
  registroRolUserMensaje: string = '';
  registroRolUserError: string = '';

  constructor(private ApiRolUserService: ApiRolUserService) { }

  ngOnInit(): void {
    this.loadRolUser();
  }

  loadRolUser(): void {
    this.ApiRolUserService.getData().subscribe({
      next: (data) => {
        this.RolUsers = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Roles de Usuario desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerRolUserPorId(id: number): void {
    this.ApiRolUserService.getById(id).subscribe({
      next: (rolUser) => {
        this.rolUserSeleccionado = { ...rolUser };
        this.modoEdicion = true;
        console.log('Rol de Usuario obtenido para edición:', this.rolUserSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el Rol de Usuario:', error);
        this.errorMessage = 'Error al cargar los detalles del Rol de Usuario para editar.';
        this.modoEdicion = false;
        this.rolUserSeleccionado = null;
      }
    });
  }

  actualizarRolUser(rolUser: IRolUser): void {
    // Asumiendo que tu backend espera un ID para la actualización
    // Deberías obtener el ID del objeto rolUser si está disponible.
    // Si no hay ID, es posible que necesites otra lógica para identificar el registro a actualizar.
    // Por ahora, este ejemplo asume que el backend usa alguna combinación de rolName y userName para la actualización.
    this.rolUserSeleccionado = { ...rolUser };
    this.modoEdicion = true;
  }

  guardarRolUser(): void {
    if (this.rolUserSeleccionado) {
      // **Importante:** Ajusta la lógica de `putData` en tu servicio
      // `ApiRolUserService` para que coincida con tu API.
      // Es posible que necesites enviar un objeto con `rolName`, `userName` y `active`.
      this.ApiRolUserService.putData(0, this.rolUserSeleccionado) // El '0' es un marcador de posición, ajusta según tu API
        .subscribe({
          next: (response) => {
            console.log('Rol de Usuario actualizado:', response);
            this.loadRolUser();
            this.modoEdicion = false;
            this.rolUserSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el Rol de Usuario:', error);
            this.errorMessage = 'Error al guardar los cambios del Rol de Usuario.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.rolUserSeleccionado = null;
  }

  eliminarRolUser(rolUser: IRolUser): void {
    if (confirm(`¿Estás seguro de que deseas eliminar la asignación del rol "${rolUser.rolName}" al usuario "${rolUser.userName}"?`)) {
      // **Importante:** Ajusta la lógica de `deleteData` en tu servicio
      // `ApiRolUserService` para que coincida con tu API.
      // Es posible que necesites enviar un objeto con `rolName` y `userName` para la eliminación.
      this.ApiRolUserService.deleteData(0) // El '0' es un marcador de posición, ajusta según tu API
        .subscribe({
          next: (response) => {
            console.log('Rol de Usuario eliminado:', response);
            this.loadRolUser();
          },
          error: (error) => {
            console.error('Error al eliminar el Rol de Usuario:', error);
            this.errorMessage = 'Error al eliminar la asignación del Rol de Usuario.';
          }
        });
    }
  }

  registrarNuevoRolUser(): void {
    this.registroRolUserMensaje = '';
    this.registroRolUserError = '';
    this.ApiRolUserService.postData(this.nuevoRolUser).subscribe({
      next: (response) => {
        console.log('Rol de Usuario registrado:', response);
        this.registroRolUserMensaje = 'Rol de Usuario registrado exitosamente.';
        this.nuevoRolUser = { rolName: '', userName: '', active: false };
        this.loadRolUser();
      },
      error: (error) => {
        console.error('Error al registrar el Rol de Usuario:', error);
        this.registroRolUserError = 'Error al registrar el Rol de Usuario.';
      }
    });
  }
}