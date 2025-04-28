import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Importa FormsModule para ngModel
import { IForm } from '../../Interfaces/iform';
import { ApiFormService } from '../../service/api.form.service'; // Ajusta la ruta a tu ApiService

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule, FormsModule], // Agrega FormsModule a los imports
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {
  forms: IForm[] = [];
  errorMessage: string = '';
  formularioSeleccionado: IForm | null = null;
  modoEdicion: boolean = false; // Para controlar si estamos en modo edición
  nuevoFormulario: { name: string; url: string; active?: boolean } = { name: '', url: '' };
  registroFormularioMensaje: string = '';
  registroFormularioError: string = '';

  constructor(private ApiFormService: ApiFormService) { }

  ngOnInit(): void {
    this.loadForms();
  }

  loadForms(): void {
    this.ApiFormService.getData().subscribe({
      next: (data) => {
        this.forms = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los formularios desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerFormularioPorId(id: number): void {
    this.ApiFormService.getById(id).subscribe({
      next: (form) => {
        this.formularioSeleccionado = { ...form }; // Crea una copia para no modificar la lista directamente
        this.modoEdicion = true;
        console.log('Formulario obtenido para edición:', this.formularioSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener el formulario:', error);
        this.errorMessage = 'Error al cargar los detalles del formulario para editar.';
        this.modoEdicion = false;
        this.formularioSeleccionado = null;
      }
    });
  }

  actualizarFormulario(form: IForm): void {
    this.obtenerFormularioPorId(form.id);
  }

  guardarFormulario(): void {
    if (this.formularioSeleccionado) {
      this.ApiFormService.putData(this.formularioSeleccionado.id, this.formularioSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Formulario actualizado:', response);
            this.loadForms(); // Recargar la lista
            this.modoEdicion = false;
            this.formularioSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar el formulario:', error);
            this.errorMessage = 'Error al guardar los cambios del formulario.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.formularioSeleccionado = null;
  }

  eliminarFormulario(form: IForm): void {
    if (confirm(`¿Estás seguro de que deseas eliminar el formulario "${form.name}"?`)) {
      this.ApiFormService.deleteData(form.id).subscribe({
        next: (response) => {
          console.log('Formulario eliminado:', response);
          this.loadForms(); // Recargar la lista de formularios después de eliminar
        },
        error: (error) => {
          console.error('Error al eliminar el formulario:', error);
          this.errorMessage = 'Error al eliminar el formulario.';
        }
      });
    }
  }

  registrarNuevoFormulario(): void {
    this.registroFormularioMensaje = '';
    this.registroFormularioError = '';
    this.ApiFormService.postData(this.nuevoFormulario).subscribe({
      next: (response) => {
        console.log('Formulario registrado:', response);
        this.registroFormularioMensaje = 'Formulario registrado exitosamente.';
        this.nuevoFormulario = { name: '', url: '' }; // Limpiar el formulario
        this.loadForms(); // Recargar la lista para mostrar el nuevo formulario
      },
      error: (error) => {
        console.error('Error al registrar formulario:', error);
        this.registroFormularioError = 'Error al registrar el formulario.';
      }
    });
  }
}