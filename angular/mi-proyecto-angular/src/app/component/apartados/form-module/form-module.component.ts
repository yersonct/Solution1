import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IFormModule } from '../../Interfaces/iform-module';
import { ApiFormModuleService } from '../../service/api.form-module.service'; // Ajusta la ruta a tu ApiService
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-module',
  imports: [CommonModule,FormsModule], // Asegúrate de incluir FormsModule si lo necesitas
  templateUrl: './form-module.component.html',
  styleUrl: './form-module.component.css',
  standalone: true
})
export class FormModuleComponent implements OnInit {
  FormModules: IFormModule[] = [];
  errorMessage: string = '';
  formModuleSeleccionado: IFormModule | null = null;
  modoEdicion: boolean = false;
  nuevoFormModule: { formName: string; moduleName: string; active?: boolean } = { formName: '', moduleName: '' };
  registroFormModuleMensaje: string = '';
  registroFormModuleError: string = '';

  constructor(private ApiFormModuleService: ApiFormModuleService) { }

  ngOnInit(): void {
    this.loadFormModule();
  }

  loadFormModule(): void {
    this.ApiFormModuleService.getData().subscribe({
      next: (data) => {
        this.FormModules = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Formularios por Módulo desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerFormModulePorId(id: number): void {
    this.ApiFormModuleService.getById(id).subscribe({
      next: (formModule) => {
        this.formModuleSeleccionado = { ...formModule };
        this.modoEdicion = true;
        console.log('Formulario por Módulo obtenido para edición:', this.formModuleSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el Formulario por Módulo:', error);
        this.errorMessage = 'Error al cargar los detalles para editar.';
        this.modoEdicion = false;
        this.formModuleSeleccionado = null;
      }
    });
  }

  actualizarFormModule(formModule: IFormModule): void {
    this.obtenerFormModulePorId(formModule.id); // Asumiendo que tu interfaz IFormModule tiene un 'id'
  }

  guardarFormModule(): void {
    if (this.formModuleSeleccionado) {
      this.ApiFormModuleService.putData(this.formModuleSeleccionado.id, this.formModuleSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Formulario por Módulo actualizado:', response);
            this.loadFormModule();
            this.modoEdicion = false;
            this.formModuleSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el Formulario por Módulo:', error);
            this.errorMessage = 'Error al guardar los cambios.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.formModuleSeleccionado = null;
  }

  eliminarFormModule(formModule: IFormModule): void {
    if (confirm(`¿Estás seguro de que deseas eliminar la relación "${formModule.formName} - ${formModule.moduleName}"?`)) {
      this.ApiFormModuleService.deleteData(formModule.id).subscribe({ // Asumiendo que tienes un 'id'
        next: (response) => {
          console.log('Relación eliminada:', response);
          this.loadFormModule();
        },
        error: (error) => {
          console.error('Error al eliminar la relación:', error);
          this.errorMessage = 'Error al eliminar la relación.';
        }
      });
    }
  }

  registrarNuevoFormModule(): void {
    this.registroFormModuleMensaje = '';
    this.registroFormModuleError = '';
    this.ApiFormModuleService.postData(this.nuevoFormModule).subscribe({
      next: (response) => {
        console.log('Relación registrada:', response);
        this.registroFormModuleMensaje = 'Relación registrada exitosamente.';
        this.nuevoFormModule = { formName: '', moduleName: '' };
        this.loadFormModule();
      },
      error: (error) => {
        console.error('Error al registrar la relación:', error);
        this.registroFormModuleError = 'Error al registrar la relación.';
      }
    });
  }
}