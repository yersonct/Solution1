import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IModule } from '../../Interfaces/imodule';
import { ApiModuleService } from '../../service/api.module.service'; //
import { FormsModule } from '@angular/forms'; // Importa FormsModule si vas a agregar formularios

@Component({
  selector: 'app-module',
  imports: [CommonModule, FormsModule], // Asegúrate de incluir FormsModule si lo necesitas
  templateUrl: './module.component.html',
  styleUrl: './module.component.css',
  standalone: true

})
export class ModuleComponent implements OnInit {
  Modules: IModule[] = [];
  errorMessage: string = ''; // Para manejar errores
  moduleSeleccionado: IModule | null = null;
  modoEdicion: boolean = false;
  nuevoModule: { name: string; active?: boolean } = { name: '' };
  registroModuleMensaje: string = '';
  registroModuleError: string = '';

  constructor(private ApiModuleService: ApiModuleService) { }

  ngOnInit(): void {
    this.loadModule();
  }

  loadModule(): void {
    this.ApiModuleService.getData().subscribe({
      next: (data) => {
        this.Modules = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Módulos desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerModulePorId(id: number): void {
    this.ApiModuleService.getById(id).subscribe({
      next: (module) => {
        this.moduleSeleccionado = { ...module };
        this.modoEdicion = true;
        console.log('Módulo obtenido para edición:', this.moduleSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el Módulo:', error);
        this.errorMessage = 'Error al cargar los detalles del módulo para editar.';
        this.modoEdicion = false;
        this.moduleSeleccionado = null;
      }
    });
  }

  actualizarModule(module: IModule): void {
    this.obtenerModulePorId(module.id); // Asumiendo que tu interfaz IModule tiene un 'id'
  }

  guardarModule(): void {
    if (this.moduleSeleccionado) {
      this.ApiModuleService.putData(this.moduleSeleccionado.id, this.moduleSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Módulo actualizado:', response);
            this.loadModule();
            this.modoEdicion = false;
            this.moduleSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el Módulo:', error);
            this.errorMessage = 'Error al guardar los cambios del módulo.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.moduleSeleccionado = null;
  }

  eliminarModule(module: IModule): void {
    if (confirm(`¿Estás seguro de que deseas eliminar el módulo "${module.name}"?`)) {
      this.ApiModuleService.deleteData(module.id).subscribe({ // Asumiendo que tienes un 'id'
        next: (response) => {
          console.log('Módulo eliminado:', response);
          this.loadModule();
        },
        error: (error) => {
          console.error('Error al eliminar el Módulo:', error);
          this.errorMessage = 'Error al eliminar el módulo.';
        }
      });
    }
  }

  registrarNuevoModule(): void {
    this.registroModuleMensaje = '';
    this.registroModuleError = '';
    this.ApiModuleService.postData(this.nuevoModule).subscribe({
      next: (response) => {
        console.log('Módulo registrado:', response);
        this.registroModuleMensaje = 'Módulo registrado exitosamente.';
        this.nuevoModule = { name: '' };
        this.loadModule();
      },
      error: (error) => {
        console.error('Error al registrar el Módulo:', error);
        this.registroModuleError = 'Error al registrar el módulo.';
      }
    });
  }
}