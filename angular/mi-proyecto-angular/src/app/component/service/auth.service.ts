import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRegisterRequest } from '../Interfaces/iperson'; // Asegúrate de crear esta interfaz
import { ILoginRequest } from '../Interfaces/i-login-request'; // Asegúrate de crear esta interfaz
import { ILoginResponse } from '../Interfaces/i-login-response'; // Asegúrate de crear esta interfaz

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7035/api/auth'; // Reemplaza con la URL de tu API

  constructor(private http: HttpClient) {}

  register(registerRequest: IRegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, registerRequest);
  }

  login(loginRequest: ILoginRequest): Observable<ILoginResponse> {
    return this.http.post<ILoginResponse>(`${this.apiUrl}/login`, loginRequest);
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
  }

  // Método para verificar si el usuario está autenticado (si el token existe)
  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}