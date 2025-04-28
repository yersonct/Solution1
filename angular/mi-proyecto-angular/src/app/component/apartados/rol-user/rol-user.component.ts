import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IRolUser } from '../../Interfaces/irol-user';
import { ApiRolUserService } from '../../service/api.rol-user.service';

@Component({
  selector: 'app-rol-user',
  imports: [CommonModule],
  templateUrl: './rol-user.component.html',
  styleUrl: './rol-user.component.css',
  standalone: true

})
export class RolUserComponent  implements OnInit{
  RolUsers: IRolUser[] = [];
  errorMessage: string = ''; // Para manejar errores

  constructor(private ApiRolUserService: ApiRolUserService) { }

  ngOnInit(): void {
    this.loadRolUser();
  }

  loadRolUser(): void {
    this.ApiRolUserService.getData().subscribe({
      next: (data) => {
        this.RolUsers = data;
      },
      error: (error) => {
        this.errorMessage = 'Error al cargar los Rol desde la API.';
        console.error('Error:', error);
      }
    });
  }
}
