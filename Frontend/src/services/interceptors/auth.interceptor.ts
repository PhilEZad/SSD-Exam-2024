import { HttpInterceptorFn } from '@angular/common/http';
import {AuthService} from '../auth.service';
import {inject} from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);

  if (req.url.endsWith('/login') || req.url.endsWith('/register')) {
    return next(req);
  }

  // Get the token from the AuthService
  const token = authService.getToken();

  // Clone the request and set the new header
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}` // Add the token in Authorization header
      }
    });
    return next(cloned); // Pass the cloned request instead of the original request to the next handler
  }

  return next(req);
};
