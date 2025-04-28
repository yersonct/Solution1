  import { CommonModule } from '@angular/common';
  import { Component, OnInit } from '@angular/core';
  import { IForm } from '../../Interfaces/iform';
  import { ApiFormService } from '../../service/api.form.service'; // Ajusta la ruta a tu ApiService



  @Component({
    selector: 'app-form',
    standalone: true,
    imports: [CommonModule], 
    templateUrl: './form.component.html',
    styleUrls: ['./form.component.css']
  })
  export class FormComponent implements OnInit {
    forms: IForm[] = [];
    errorMessage: string = ''; // Para manejar errores

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
  }