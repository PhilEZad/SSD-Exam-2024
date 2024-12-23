import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {HttpInterceptorFn, provideHttpClient, withInterceptors} from '@angular/common/http';

const appInterceptors: HttpInterceptorFn[] = [
  // Add any interceptors here
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors(appInterceptors)),

  ]
};
