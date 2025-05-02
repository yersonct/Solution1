import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRegisterRequest } from '../Interfaces/iperson'; // Asegúrate de crear esta interfaz
import { ILoginRequest } from '../Interfaces/i-login-request'; // Asegúrate de crear esta interfaz
import { ILoginResponse } from '../Interfaces/i-login-response'; // Asegúrate de crear esta interfaz
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

interface LoginResponse {
  token: string;
  // expiration: Date; // Ya no es estrictamente necesario si decodificamos el token
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7035/api'; // Reemplaza con la URL de tu API

  constructor(private http: HttpClient, private router: Router) {}

  register(registerRequest: IRegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, registerRequest);
  }

  login(loginRequest: ILoginRequest): Observable<ILoginResponse> {
    return this.http.post<ILoginResponse>(`${this.apiUrl}/auth/login`, loginRequest).pipe(
      tap((response) => {
        this.saveToken(response.token);
      })
    );
  }

  // Método para guardar el token JWT en el almacenamiento local
  saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  // Método para obtener el token JWT del almacenamiento local
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  // Método para eliminar el token JWT del almacenamiento local (al cerrar sesión)
  removeToken(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }

  // Método para verificar si el usuario está autenticado (si el token existe y no ha expirado)
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.exp > Date.now() / 1000; // Compara la expiración (en segundos) con la hora actual (en segundos)
    } catch (error) {
      return false; // Token inválido
    }
  }

  // Nuevo método para verificar si el token está próximo a expirar
  isTokenExpiringSoon(thresholdSeconds: number = 60): boolean {
    const token = this.getToken();
    if (!token) {
      return true; // Si no hay token, se considera expirado
    }
    try {
      const decodedToken: any = jwtDecode(token);
      const expirationTime = decodedToken.exp * 1000; // Convertir a milisegundos
      const currentTime = Date.now();
      return (expirationTime - currentTime) < (thresholdSeconds * 1000);
    } catch (error) {
      return true; // Si el token es inválido, se considera expirado
    }
  }
}