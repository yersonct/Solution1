// component/registro/registro.component.ts
import { Component, OnInit } from '@angular/core';
import { AuthService } from './../service/auth.service';
import { Router } from '@angular/router';
import { IRegisterRequest } from './../Interfaces/iperson';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.css'],
})
export class RegisterComponent implements OnInit {
  registerData: IRegisterRequest = {
    username: '',
    password: '',
    person: {
      name: '',
      lastName: '',
      document: '',
      phone: '',
      email: '',
      active: true,
      id: 0
    },
  };
  registrationError: string = '';
  registrationSuccess: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/app']);
    }
  }

  onSubmit(): void {
    this.registrationError = '';
    this.registrationSuccess = '';

    this.authService.register(this.registerData).subscribe({
      next: (response) => {
        console.log('Registro exitoso:', response);
        this.registrationSuccess = 'Registro exitoso. ¡Puedes iniciar sesión ahora!';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        console.error('Error de registro:', error);
        if (error?.error?.errors) {
          this.registrationError = Object.values(error.error.errors).flat().join(' ');
        } else if (error?.error?.message) {
          this.registrationError = error.error.message;
        } else {
          this.registrationError = 'Error al registrar el usuario. Por favor, inténtalo de nuevo.';
        }
      },
    });
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }
}