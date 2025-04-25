import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

// Ya no importamos HomeComponent y LoginComponent aquí

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    // Si AppComponent no es standalone, podrías importar aquí otros módulos que necesite
  ],
  declarations: [],
  providers: [],
  // bootstrap: [AppComponent] // <-- Elimina esta línea
})
export class AppModule { }