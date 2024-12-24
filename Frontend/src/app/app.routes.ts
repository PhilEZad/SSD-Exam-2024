import { Routes } from '@angular/router';
import {HomePageComponent} from './home-page/home-page.component';
import {LoginPageComponent} from './login-page/login-page.component';
import {RegistrationPageComponent} from './registration-page/registration-page.component';

export const routes: Routes = [
  // Add your routes here
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: 'home', pathMatch: 'full', component: HomePageComponent},
  {path: 'login', pathMatch: 'full', component: LoginPageComponent},
  {path: 'register', pathMatch: 'full', component: RegistrationPageComponent},
  {path: '**', redirectTo: '/home'},


];
