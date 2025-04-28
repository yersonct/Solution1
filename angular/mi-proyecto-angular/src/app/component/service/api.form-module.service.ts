import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ApiFormModuleService {
  private apiUrl = environment.apiUrl;

  constructor(private https: HttpClient) { }

  // GET: Obtiene todos los elementos
  getData(): Observable<any> {
    console.log(`URL: ${this.apiUrl}FormModules`);
    return this.https.get<any>('/api/FormModules'); // 👈 Aquí corriges
  }
  // getByNombre(nombre: string): Observable<any[]> {
  //   const params = new HttpParams().set('name', nombre);
  //   return this.https.get<any[]>('/api/Forms', { params });
  // }
  // GET: Obtiene un elemento por su ID
  getById(id: number): Observable<any> {
    return this.https.get<any>(`/api/FormModules/${id}`); // 👈 Corrige aquí también
  }

  // POST: Crea un nuevo elemento
  postData(data: any): Observable<any> {
    return this.https.post<any>('/api/FormModules', data); // 👈 Corrige aquí
  }

  // PUT: Actualiza un elemento
  putData(id: number, data: any): Observable<any> {
    return this.https.put<any>(`/api/FormModules/${id}`, data); // 👈 Corrige aquí
  }

  createUser(userData: any): Observable<any> {
    return this.https.post<any>('/api/users', userData); // O la ruta correcta para crear usuarios
  }
  // DELETE: Elimina un elemento
  deleteData(id: number): Observable<any> {
    return this.https.delete<any>(`/api/FormModules/${id}`); // 👈 Corrige aquí
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