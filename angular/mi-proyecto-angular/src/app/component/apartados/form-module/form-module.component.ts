import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-form-module',
  imports: [FormBuilder],
  templateUrl: './form-module.component.html',
  styleUrl: './form-module.component.css',
  standalone: true
})
export class FormModuleComponent {
  public titulo: string = "Formularios y Modulos"
}
