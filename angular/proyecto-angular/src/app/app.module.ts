import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from '../app/app.component'; // Importa tu AppComponent
import { provideRouter, Routes } from '@angular/router';
import { importProvidersFrom } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

const routes: Routes = [
    // tus rutas
];

bootstrapApplication(AppComponent, {
    providers: [
        provideRouter(routes),
        importProvidersFrom(ReactiveFormsModule, HttpClientModule),
    ]
}).catch(err => console.error(err));