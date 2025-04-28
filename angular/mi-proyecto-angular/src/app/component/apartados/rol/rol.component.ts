import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IRol } from '../../Interfaces/irol';
import { ApiRolService } from '../../service/api.rol.service'; // Ajusta la ruta a tu ApiService
import { FormsModule } from '@angular/forms'; // Importa FormsModule si vas a agregar formularios

@Component({
  selector: 'app-rol',
  standalone: true,
  imports: [CommonModule, FormsModule], // Asegúrate de incluir FormsModule si lo necesitas
  templateUrl: './rol.component.html',
  styleUrl: './rol.component.css',

})
export class RolComponent implements OnInit {
  Roles: IRol[] = [];
  errorMessage: string = ''; // Para manejar errores
  rolSeleccionado: IRol | null = null;
  modoEdicion: boolean = false;
  nuevoRol: { name: string; description: string; active?: boolean } = { name: '', description: '' };
  registroRolMensaje: string = '';
  registroRolError: string = '';

  constructor(private ApiRolService: ApiRolService) { }

  ngOnInit(): void {
    this.loadRol();
  }

  loadRol(): void {
    this.ApiRolService.getData().subscribe({
      next: (data) => {
        this.Roles = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Roles desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerRolPorId(id: number): void {
    this.ApiRolService.getById(id).subscribe({
      next: (rol) => {
        this.rolSeleccionado = { ...rol };
        this.modoEdicion = true;
        console.log('Rol obtenido para edición:', this.rolSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el Rol:', error);
        this.errorMessage = 'Error al cargar los detalles del rol para editar.';
        this.modoEdicion = false;
        this.rolSeleccionado = null;
      }
    });
  }

  actualizarRol(rol: IRol): void {
    this.obtenerRolPorId(rol.id); // Asumiendo que tu interfaz IRol tiene un 'id'
  }

  guardarRol(): void {
    if (this.rolSeleccionado) {
      this.ApiRolService.putData(this.rolSeleccionado.id, this.rolSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Rol actualizado:', response);
            this.loadRol();
            this.modoEdicion = false;
            this.rolSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el Rol:', error);
            this.errorMessage = 'Error al guardar los cambios del rol.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.rolSeleccionado = null;
  }

  eliminarRol(rol: IRol): void {
    if (confirm(`¿Estás seguro de que deseas eliminar el rol "${rol.name}"?`)) {
      this.ApiRolService.deleteData(rol.id).subscribe({ // Asumiendo que tienes un 'id'
        next: (response) => {
          console.log('Rol eliminado:', response);
          this.loadRol();
        },
        error: (error) => {
          console.error('Error al eliminar el Rol:', error);
          this.errorMessage = 'Error al eliminar el rol.';
        }
      });
    }
  }

  registrarNuevoRol(): void {
    this.registroRolMensaje = '';
    this.registroRolError = '';
    this.ApiRolService.postData(this.nuevoRol).subscribe({
      next: (response) => {
        console.log('Rol registrado:', response);
        this.registroRolMensaje = 'Rol registrado exitosamente.';
        this.nuevoRol = { name: '', description: '' };
        this.loadRol();
      },
      error: (error) => {
        console.error('Error al registrar el Rol:', error);
        this.registroRolError = 'Error al registrar el rol.';
      }
    });
  }
}