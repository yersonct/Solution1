import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IPerson } from '../../Interfaces/iperson';
import { ApiPersonService } from '../../service/api.person.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-person',
  imports: [CommonModule, FormsModule],
  templateUrl: './person.component.html',
  styleUrl: './person.component.css',
  standalone: true

})
export class PersonComponent implements OnInit {
  Persons: IPerson[] = [];
  errorMessage: string = '';
  personSeleccionado: IPerson | null = null;
  modoEdicion: boolean = false;
  nuevoPerson: { name: string; lastName: string; document: string; phone: string; email: string; active?: boolean } = { name: '', lastName: '', document: '', phone: '', email: '' };
  registroPersonMensaje: string = '';
  registroPersonError: string = '';

  constructor(private ApiPersonService: ApiPersonService) { }

  ngOnInit(): void {
    this.loadPerson();
  }

  loadPerson(): void {
    this.ApiPersonService.getData().subscribe({
      next: (data) => {
        this.Persons = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar las personas desde la API.';
        console.error('Error:', error);
      }
    });
  }

  obtenerPersonPorId(id: number): void {
    this.ApiPersonService.getById(id).subscribe({
      next: (person) => {
        this.personSeleccionado = { ...person };
        this.modoEdicion = true;
        console.log('Persona obtenida para edición:', this.personSeleccionado);
      },
      error: (error) => {
        console.error('Error al obtener la persona:', error);
        this.errorMessage = 'Error al cargar los detalles de la persona para editar.';
        this.modoEdicion = false;
        this.personSeleccionado = null;
      }
    });
  }

  actualizarPerson(person: IPerson): void {
    this.obtenerPersonPorId(person.id);
  }

  guardarPerson(): void {
    if (this.personSeleccionado) {
      this.ApiPersonService.putData(this.personSeleccionado.id, this.personSeleccionado)
        .subscribe({
          next: (response) => {
            console.log('Persona actualizada:', response);
            this.loadPerson();
            this.modoEdicion = false;
            this.personSeleccionado = null;
          },
          error: (error) => {
            console.error('Error al actualizar la persona:', error);
            this.errorMessage = 'Error al guardar los cambios de la persona.';
          }
        });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.personSeleccionado = null;
  }

  eliminarPerson(person: IPerson): void {
    if (confirm(`¿Estás seguro de que deseas eliminar a "${person.name} ${person.lastName}"?`)) {
      this.ApiPersonService.deleteData(person.id).subscribe({
        next: (response) => {
          console.log('Persona eliminada:', response);
          this.loadPerson();
        },
        error: (error) => {
          console.error('Error al eliminar la persona:', error);
          this.errorMessage = 'Error al eliminar la persona.';
        }
      });
    }
  }

  registrarNuevaPerson(): void {
    this.registroPersonMensaje = '';
    this.registroPersonError = '';
    this.ApiPersonService.postData(this.nuevoPerson).subscribe({
      next: (response) => {
        console.log('Persona registrada:', response);
        this.registroPersonMensaje = 'Persona registrada exitosamente.';
        this.nuevoPerson = { name: '', lastName: '', document: '', phone: '', email: '' };
        this.loadPerson();
      },
      error: (error) => {
        console.error('Error al registrar la persona:', error);
        this.registroPersonError = 'Error al registrar la persona.';
      }
    });
  }
}