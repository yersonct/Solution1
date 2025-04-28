import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IRol } from '../../Interfaces/irol';
import { ApiRolService } from '../../service/api.rol.service'; // Ajusta la ruta a tu ApiService


@Component({
  selector: 'app-rol',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './rol.component.html',
  styleUrl: './rol.component.css',

})
export class RolComponent implements OnInit {
  Roles: IRol[] = [];
  errorMessage: string = ''; // Para manejar errores

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
        this.errorMessage = 'Error al cargar los Rol desde la API.';
        console.error('Error:', error);
      }
    });
  }
}
