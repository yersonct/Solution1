import { Component } from '@angular/core';

@Component({
  selector: 'app-module',
  imports: [],
  templateUrl: './module.component.html',
  styleUrl: './module.component.css',
  standalone: true

})
export class ModuleComponent {
   public titulo: string = "Modulos"
}
