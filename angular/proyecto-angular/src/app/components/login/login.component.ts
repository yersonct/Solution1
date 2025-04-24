import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms'; // Importa ReactiveFormsModule
import { CommonModule } from '@angular/common'; // Importa CommonModule

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule], // Agrega ReactiveFormsModule y CommonModule a los imports
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    loginForm!: FormGroup;
    loading = false;
    error = '';

    constructor(private fb: FormBuilder) { }

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
        console.log(this.loginForm.value);
        this.loading = false; //Es importante cambiar el estado de loading
    }
}