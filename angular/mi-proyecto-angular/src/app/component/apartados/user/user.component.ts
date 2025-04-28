import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IUser } from '../../Interfaces/iuser';
import { ApiUserService } from '../../service/api.user.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user',
  imports: [CommonModule, FormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
  standalone: true

})
export class UserComponent implements OnInit {
  Users: IUser[] = [];
  errorMessage: string = ''; // Para manejar errores
  userSeleccionado: IUser | null = null;
  modoEdicion: boolean = false;
  nuevoUser: { userName: string; password: string; personId?: number; personName?: string; active: boolean } = { userName: '', password: '', active: false };
  registroUserMensaje: string = '';
  registroUserError: string = '';

  constructor(private ApiUserService: ApiUserService) { }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.ApiUserService.getData().subscribe({
      next: (data) => {
        this.Users = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los usuarios desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerUsuarioPorId(id: number): void {
    this.ApiUserService.getById(id).subscribe({
      next: (user) => {
        this.userSeleccionado = { ...user };
        this.modoEdicion = true;
        console.log('Usuario obtenido para edición:', this.userSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el usuario:', error);
        this.errorMessage = 'Error al cargar los detalles del usuario para editar.';
        this.modoEdicion = false;
        this.userSeleccionado = null;
      }
    });
  }

  actualizarUsuario(user: IUser): void {
    this.obtenerUsuarioPorId(user.id);
  }

  guardarUsuario(): void {
    if (this.userSeleccionado) {
      this.ApiUserService.putData(this.userSeleccionado.id, this.userSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Usuario actualizado:', response);
            this.loadUser();
            this.modoEdicion = false;
            this.userSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el usuario:', error);
            this.errorMessage = 'Error al guardar los cambios del usuario.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.userSeleccionado = null;
  }

  eliminarUsuario(user: IUser): void {
    if (confirm(`¿Estás seguro de que deseas eliminar el usuario "${user.userName}"?`)) {
      this.ApiUserService.deleteData(user.id).subscribe({
        next: (response) => {
          console.log('Usuario eliminado:', response);
          this.loadUser();
        },
        error: (error) => {
          console.error('Error al eliminar el usuario:', error);
          this.errorMessage = 'Error al eliminar el usuario.';
        }
      });
    }
  }

  registrarNuevoUsuario(): void {
    this.registroUserMensaje = '';
    this.registroUserError = '';
    this.ApiUserService.postData(this.nuevoUser).subscribe({
      next: (response) => {
        console.log('Usuario registrado:', response);
        this.registroUserMensaje = 'Usuario registrado exitosamente.';
        this.nuevoUser = { userName: '', password: '', active: false };
        this.loadUser();
      },
      error: (error) => {
        console.error('Error al registrar el usuario:', error);
        this.registroUserError = 'Error al registrar el usuario.';
      }
    });
  }
}