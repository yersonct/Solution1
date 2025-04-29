import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';
import { ILoginRequest } from '../Interfaces/i-login-request';
import { FormsModule } from '@angular/forms'; // Importa FormsModule
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true, // Asegúrate de que esto esté aquí
  imports: [FormsModule, CommonModule], // Agrega FormsModule a los imports
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'], // O .css
})
export class LoginComponent implements OnInit {
  loginData: ILoginRequest = {
    username: '',
    password: '',
  };
  loginError: string = '';

  @Output() loginSuccess = new EventEmitter<void>();

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.loginError = '';

    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        console.log('Inicio de sesión exitoso:', response);
        this.authService.saveToken(response.token);
        this.loginSuccess.emit();
      },
      error: (error) => {
        console.error('Error de inicio de sesión:', error);
        if (error?.error?.message) {
          this.loginError = error.error.message;
        } else {
          this.loginError = 'Credenciales inválidas. Por favor, inténtalo de nuevo.';
        }
      },
    });
  }
}