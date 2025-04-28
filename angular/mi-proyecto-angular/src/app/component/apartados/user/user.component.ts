import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IUser } from '../../Interfaces/iuser';
import { ApiUserService } from '../../service/api.user.service';

@Component({
  selector: 'app-user',
  imports: [CommonModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
  standalone: true

})
export class UserComponent implements OnInit {
   Users: IUser[] = [];
   errorMessage: string = ''; // Para manejar errores
 
   constructor(private ApiUserService: ApiUserService) { }
 
   ngOnInit(): void {
     this.loadUser();
   }
 
   loadUser(): void {
     this.ApiUserService.getData().subscribe({
       next: (data) => {
         this.Users = data;
       },
       error: (error) => {
         this.errorMessage = 'Error al cargar los Rol desde la API.';
         console.error('Error:', error);
       }
     });
   }
}
