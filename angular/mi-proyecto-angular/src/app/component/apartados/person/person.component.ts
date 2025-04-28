import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IPerson } from '../../Interfaces/iperson';
import { ApiPersonService } from '../../service/api.person.service';

@Component({
  selector: 'app-person',
  imports: [CommonModule],
  templateUrl: './person.component.html',
  styleUrl: './person.component.css',
  standalone: true

})
export class PersonComponent implements OnInit {
  Persons: IPerson[] = [];
  errorMessage: string = ''; // Para manejar errores

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
        this.errorMessage = 'Error al cargar los person desde la API.';
        console.error('Error:', error);
      }
    });
  }
}
