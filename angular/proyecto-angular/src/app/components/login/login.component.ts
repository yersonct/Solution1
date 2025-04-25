import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router'; // Importa el Router
import { AuthService } from '../services/auth.service'; 


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private router: Router, // Inyecta el Router
    private authService: AuthService // Inyecta el servicio de autenticación
  ) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    this.loading = true;
    this.error = '';
    if (this.loginForm.invalid) {
      this.loading = false;
      return;
    }
    // Lógica de inicio de sesión aquí
    const { username, password } = this.loginForm.value;
    this.authService.login(username, password).subscribe({
      next: () => {
        this.router.navigate(['/']); // Redirige a la página principal
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Credenciales inválidas';
        this.loading = false;
      }
    });
  }
}
