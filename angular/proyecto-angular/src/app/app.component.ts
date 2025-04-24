import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Importante para usar directivas como *ngIf
import { ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from "./components/home/home.component";
import { LoginComponent } from "./components/login/login.component";

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, HomeComponent, LoginComponent], // Importa los módulos necesarios aquí
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    // ... tu código del componente
    title = 'Bienvenido a la Página de Inicio';
}