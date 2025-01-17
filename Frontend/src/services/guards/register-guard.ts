import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {AuthService} from '../auth.service';
import {map} from 'rxjs';

export const registerGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.isAuthenticated$.pipe(
    map(isAuthenticated => {
      if (!isAuthenticated) {
        return true;
      }
      return router.createUrlTree(['/home']);
    })
  );
}
