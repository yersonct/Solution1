import { Injectable } from '@angular/core';
import { ServiceGeneralService } from '../../services/service-general.service';
import { Observable } from 'rxjs';
import { Form } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormServiceService {

  private endpoint = 'Forms';
  
    constructor(private api: ServiceGeneralService) {}
  
    getAll(): Observable<Form[]> {
      return this.api.get<Form[]>(this.endpoint);
    }
  
    getById(id: number): Observable<Form> {
      return this.api.getById<Form>(this.endpoint, id);
    }
  
    create(data: Form): Observable<Form> {
      return this.api.post<Form>(this.endpoint, data);
    }
  
    update(id: number, data: Form): Observable<Form> {
      return this.api.put<Form>(this.endpoint, id, data);
    }
  
    delete(id: number): Observable<any> {
      return this.api.delete(this.endpoint, id);
    }

    deleteLogic(id: number): Observable<Form> {
      return this.api.deleteLogic<Form>(this.endpoint, id);
    }

    // restore(id: number): Observable<Form> {
    //   return this.api.patchRestore<Form>(this.endpoint, id, {});
    // }

    // restore(id: number): Observable<Form> {
    //   return this.api.patchRestore<Form>(this.endpoint, id);
    // }
}
