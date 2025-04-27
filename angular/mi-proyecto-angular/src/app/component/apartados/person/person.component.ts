import { Component } from '@angular/core';

@Component({
  selector: 'app-person',
  imports: [],
  templateUrl: './person.component.html',
  styleUrl: './person.component.css',
  standalone: true

})
export class PersonComponent {
   public titulo: string = "Personas"
}
