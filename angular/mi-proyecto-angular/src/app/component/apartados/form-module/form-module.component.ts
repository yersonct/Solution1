import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IFormModule } from '../../Interfaces/iform-module';
import { ApiFormModuleService } from '../../service/api.form-module.service'; // Ajusta la ruta a tu ApiService

@Component({
  selector: 'app-form-module',
  imports: [CommonModule],
  templateUrl: './form-module.component.html',
  styleUrl: './form-module.component.css',
  standalone: true
})
export class FormModuleComponent implements OnInit {
  FormModules: IFormModule[] = [];
  errorMessage: string = ''; // Para manejar errores
  
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
          this.errorMessage = 'Error al cargar los Rol desde la API.';
          console.error('Error:', error);
        }
      });
    }
}
