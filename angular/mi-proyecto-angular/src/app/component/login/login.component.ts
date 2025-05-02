// component/login/login.component.ts
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from './../service/auth.service';
import { Router } from '@angular/router';
import { ILoginRequest } from './../Interfaces/i-login-request';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginData: ILoginRequest = {
    username: '',
    password: '',
  };
  loginError: string = '';

  @Output() loginSuccess = new EventEmitter<void>();

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/app']);
    }
  }

  onSubmit(): void {
    this.loginError = '';

    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        console.log('Inicio de sesión exitoso:', response);
        this.authService.saveToken(response.token);
        this.loginSuccess.emit(); // Emite el evento de inicio de sesión exitoso
        this.router.navigate(['/app']); // Redirige directamente al '/app' después del login
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

  navigateToRegister(): void {
    this.router.navigate(['/registro']);
  }
}