import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, ROUTES } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
 import { HttpClientModule } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter([]),
    importProvidersFrom(BrowserModule, ReactiveFormsModule, HttpClientModule),
  ],
};