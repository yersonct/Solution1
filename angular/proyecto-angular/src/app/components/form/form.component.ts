import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormServiceService } from '../services/form-service.service';
import { Form } from '../../models/form';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,FormsModule],
  templateUrl: './form.component.html',
  styleUrl: './form.component.css'
})
export class FormComponent {
  formForm!: FormGroup;
  forms: Form [] = [];
  constructor(private formBuilder: FormBuilder, private formService : FormServiceService) {

    
   }

    ngOnInit() {
        this.loadForms();
        this.formForm = this.formBuilder.group({
          id: [null],
          name: ['',Validators.required],
          url: ['',Validators.required],
          active: [true]
      });
    } 


 

    loadForms(): void {
      this.formService.getAll()
        .subscribe({
          next: (data: any) => { // Especificamos que 'data' es de tipo 'any' para acceder a '$values'
            this.forms = data; // Accedemos al array de formularios dentro de la propiedad '$values'
          },
          error: (err) => console.error('Error al cargar formularios:', err)
        });
    }

  
}
