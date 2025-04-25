import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public titulo: string = 'Bienvenido a la Página de Inicio';
  public fechaActual: Date;
  constructor() {
    this.fechaActual = new Date();
  }

  ngOnInit(): void {
    // Este método se ejecuta cuando el componente se inicializa.
    // Aquí puedes realizar tareas de inicialización, como cargar datos.
    console.log('HomeComponent inicializado');
  }



  public obtenerFechaFormateada(): string {
    return this.fechaActual.toLocaleDateString('es-ES', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }


}