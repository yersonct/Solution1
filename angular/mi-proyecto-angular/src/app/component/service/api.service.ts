import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // GET: Obtiene todos los elementos (ejemplo: /data)
  getData(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/data`);
  }

  // GET: Obtiene un elemento por su ID (ejemplo: /items/{id})
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/items/${id}`);
  }

  // POST: Crea un nuevo elemento (ejemplo: /items)
  postData(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/items`, data);
  }

  // PUT: Actualiza un elemento existente (ejemplo: /items/{id})
  putData(id: number, data: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/items/${id}`, data);
  }

  // DELETE: Elimina un elemento por su ID (ejemplo: /items/{id})
  deleteData(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/items/${id}`);
  }

  // GET: Obtiene datos con parámetros (ejemplo: /users?page=1&limit=10)
  getDataWithParams(params: any): Observable<any> {
    let httpParams = new HttpParams();
    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        httpParams = httpParams.set(key, params[key]);
      }
    }
    return this.http.get<any>(`${this.apiUrl}/users`, { params: httpParams });
  }
}