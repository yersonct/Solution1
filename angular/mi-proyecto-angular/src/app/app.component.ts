// app.component.ts
import { Component, OnInit } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './component/service/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    // No necesitas importar LoginComponent, MenuLateralComponent ni los componentes de los apartados aquí
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'registro';
  isLoggedIn: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    // this.isLoggedIn = this.authService.isAuthenticated();
    // if (this.isLoggedIn) {
    //   this.router.navigate(['/app']);o
  
    // } else {
    //   this.router.navigate(['/login']);
    // }
  }
}