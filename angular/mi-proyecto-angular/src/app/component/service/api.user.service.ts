import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiUserService {
  private apiUrl = environment.apiUrl;

  constructor(private https: HttpClient) { }

  // GET: Obtiene todos los elementos
  getData(): Observable<any> {
    console.log(`URL: ${this.apiUrl}Users`);
    return this.https.get<any>('/api/Users'); // 👈 Aquí corriges
  }

  // GET: Obtiene un elemento por su ID
  getById(id: number): Observable<any> {
    return this.https.get<any>(`/api/Users/${id}`); // 👈 Corrige aquí también
  }

  // POST: Crea un nuevo elemento
  postData(data: any): Observable<any> {
    return this.https.post<any>('/api/Users', data); // 👈 Corrige aquí
  }

  // PUT: Actualiza un elemento
  putData(id: number, data: any): Observable<any> {
    return this.https.put<any>(`/api/Users/${id}`, data); // 👈 Corrige aquí
  }

  // DELETE: Elimina un elemento
  deleteData(id: number): Observable<any> {
    return this.https.delete<any>(`/api/Users/${id}`); // 👈 Corrige aquí
  }

  // GET: Obtiene datos con parámetros
  getDataWithParams(params: any): Observable<any> {
    let httpParams = new HttpParams();
    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        httpParams = httpParams.set(key, params[key]);
      }
    }
    return this.https.get<any>('/api/users', { params: httpParams }); // 👈 Corrige aquí
  }
}
