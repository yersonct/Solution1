// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { BehaviorSubject, Observable, throwError } from 'rxjs';
// import { tap, catchError, map } from 'rxjs/operators';
// import { Router } from '@angular/router';

// interface LoginResponse {
//   token: string;
//   expiration: string; // O Date si lo parseas
// }

// interface User {
//   username: string;
//   token: string | null;
// }

// @Injectable({
//   providedIn: 'root'
// })
// export class AuthService {
//   private currentUserSubject = new BehaviorSubject<User | null>(this.getCurrentUserFromStorage());
//   public currentUser = this.currentUserSubject.asObservable();
//   private apiUrl = 'https://localhost:7035/api'; // Reemplaza con tu URL de la API

//   constructor(
//     private http: HttpClient,
//     private router: Router
//   ) {}

//   login(credentials: any): Observable<boolean> {
//     return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
//       .pipe(
//         map(response => {
//           if (response && response.token) {
//             const user: User = { username: credentials.username, token: response.token };
//             localStorage.setItem('currentUser', JSON.stringify(user));
//             this.currentUserSubject.next(user);
//             return true;
//           } else {
//             this.currentUserSubject.next(null);
//             return false;
//           }
//         }),
//         catchError(err => {
//           this.currentUserSubject.next(null);
//           return throwError(() => 'Credenciales inválidas'); // Mensaje genérico para el usuario
//         })
//       );
//   }

//   logout(): void {
//     localStorage.removeItem('currentUser');
//     this.currentUserSubject.next(null);
//     this.router.navigate(['/login']);
//   }

//   getCurrentUserValue(): User | null {
//     return this.currentUserSubject.value;
//   }

//   private getCurrentUserFromStorage(): User | null {
//     const storedUser = localStorage.getItem('currentUser');
//     return storedUser ? JSON.parse(storedUser) : null;
//   }

//   isAuthenticated(): boolean {
//     return !!this.getCurrentUserValue()?.token;
//   }

//   getToken(): string | null {
//     return this.getCurrentUserValue()?.token || null;
//   }
// }