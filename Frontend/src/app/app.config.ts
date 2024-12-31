import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {HttpInterceptorFn, provideHttpClient, withInterceptors} from '@angular/common/http';
import {provideAnimations} from '@angular/platform-browser/animations';
import {backendUrlInterceptor} from '../services/interceptors/backend-url.interceptor';
import {authInterceptor} from '../services/interceptors/auth.interceptor';

const appInterceptors: HttpInterceptorFn[] = [
  backendUrlInterceptor,
  authInterceptor
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors(appInterceptors)),
    provideAnimations()

  ]
};
