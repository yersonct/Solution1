import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; // Importa HttpClientModule

import { AppComponent } from '../../app.component';
// ... otros imports

@NgModule({
  declarations: [
    AppComponent,
    // ... otros componentes
  ],
  imports: [
    BrowserModule,
    HttpClientModule, // Agrega HttpClientModule a los imports
    // ... otros módulos
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }