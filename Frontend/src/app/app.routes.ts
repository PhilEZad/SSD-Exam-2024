import { Routes } from '@angular/router';
import {HomePageComponent} from './home-page/home-page.component';
import {LoginPageComponent} from './login-page/login-page.component';
import {RegistrationPageComponent} from './registration-page/registration-page.component';
import {homeGuard} from '../services/guards/home-guard';
import {loginGuard} from '../services/guards/login-guard';
import {registerGuard} from '../services/guards/register-guard';

export const routes: Routes = [
  // Add your routes here
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: 'home', pathMatch: 'full', component: HomePageComponent, canActivate: [homeGuard]},
  {path: 'login', pathMatch: 'full', component: LoginPageComponent, canActivate: [loginGuard]},
  {path: 'register', pathMatch: 'full', component: RegistrationPageComponent, canActivate: [registerGuard]},
  {path: '**', redirectTo: '/home'},


];
