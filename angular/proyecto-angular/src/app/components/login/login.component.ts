import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loading = false;
  error: string = '';

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    // Simulación de login
    const { username, password } = this.loginForm.value;
    console.log('Login con:', username, password);

    setTimeout(() => {
      this.loading = false;
      if (username === 'admin' && password === 'admin') {
        alert('Login exitoso!');
      } else {
        this.error = 'Usuario o contraseña incorrectos.';
      }
    }, 1500);
  }
}
