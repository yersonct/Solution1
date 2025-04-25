import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

interface User {
  username: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser: User | null = null;
  private apiUrl = 'http://localhost:3000/auth'; // Reemplaza con tu URL de la API

  constructor(
    private http: HttpClient,
    private router: Router
    ) {}

  // Método para iniciar sesión
  login(username: string, password: string): Observable<void> {
    // En un escenario real, aquí harías una petición HTTP a tu API
    // para verificar las credenciales.  Para este ejemplo, usaré datos de prueba.
    return of({ username: 'testuser', token: 'fake-token' }).pipe( //Simulo la respuesta de la API
      tap((response: any) => { // Type assertion added
        if (username === 'testuser' && password === 'password') {
          this.currentUser = { username, token: response.token };
          localStorage.setItem('currentUser', JSON.stringify(this.currentUser));
        } else {
          throw new Error('Credenciales inválidas'); // Simulate login failure
        }
      }),
      catchError(err => {
        return throwError(() => err);
      })
    );
  }

  // Método para cerrar sesión
  logout(): void {
    this.currentUser = null;
    localStorage.removeItem('currentUser');
    this.router.navigate(['/login']); // Redirige al login
  }

  // Método para obtener el usuario actual
  getCurrentUser(): User | null {
    if (this.currentUser)
      return this.currentUser;
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUser = JSON.parse(storedUser);
    }
    return this.currentUser;
  }

  // Método para verificar si el usuario está autenticado
  isAuthenticated(): boolean {
    return !!this.getCurrentUser();
  }

    // Método para obtener el token
    getToken(): string | null {
      const user = this.getCurrentUser();
      return user ? user.token : null;
    }
}
