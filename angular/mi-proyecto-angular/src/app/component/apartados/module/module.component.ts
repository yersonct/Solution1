import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IModule } from '../../Interfaces/imodule';
import { ApiModuleService } from '../../service/api.module.service'; //

@Component({
  selector: 'app-module',
  imports: [CommonModule],
  templateUrl: './module.component.html',
  styleUrl: './module.component.css',
  standalone: true

})
export class ModuleComponent implements OnInit{
   Modules: IModule[] = [];
     errorMessage: string = ''; // Para manejar errores
   
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
           this.errorMessage = 'Error al cargar los Rol desde la API.';
           console.error('Error:', error);
         }
       });
     }
}
