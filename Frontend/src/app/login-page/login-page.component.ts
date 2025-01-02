import { Component } from '@angular/core';
import {CommonModule} from '@angular/common';
import {Router, RouterLink} from '@angular/router';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {catchError, of, tap} from 'rxjs';
import {LoginDto} from '../../domain/domain';
import {Hasher} from '../../services/security/hasher';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './login-page.component.html',
  standalone: true,
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  loginForm: FormGroup;
  loginAttempted: boolean = false;
  loginError: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }


  async onSubmit() {
    this.loginAttempted = true; // Mark as attempted

    if (this.loginForm.valid) {
      const {username, password} = this.loginForm.value;


      const hashedPassword = await Hasher.hashPromise(password, username);

      const request: LoginDto = {username: username, plainPassword: hashedPassword}; // Create the request object

      // Call the backend service to authenticate
      this.authService.login(request).pipe(
        catchError((error) => {
          this.loginError = true; // Show error message
          console.error('Login error:', error); // Optionally log the error
          return of(false); // Return an observable to complete the stream
        })
      ).subscribe((success) => {
        if (success) {
          console.log('Login success:', success);
          this.router.navigate(['home']); // Redirect to home on success
        }
        else {
          this.loginError = true;
        }
      });
    }
  }
}
