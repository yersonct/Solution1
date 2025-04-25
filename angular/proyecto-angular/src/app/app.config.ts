import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router'; // Aunque no se declare nada aquí, podría ser necesario si otros módulos lo usan

import { AppComponent } from './app.component'; // Ya no se declara aquí
import { LoginComponent } from './components/login/login.component'; // Ya no se declara aquí
import { HomeComponent } from './components/home/home.component'; // Ya no se declara aquí
import { AuthGuard } from './guards/auth.guard';
import { AuthService } from './services/auth.service';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';

@NgModule({
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule // Importa RouterModule
  ],
  providers: [
    AuthGuard,
    AuthService,
    { provide: JWT_OPTIONS, useValue: {} },
    JwtHelperService
  ],
  bootstrap: [AppComponent] // AppComponent se usa para el bootstrap
})
export class AppModule { }