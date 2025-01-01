import {Component} from '@angular/core';
import {RegisterDto} from '../../domain/domain';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {AuthService} from '../../services/auth.service';
import {Router, RouterLink} from '@angular/router';
import {catchError, of, tap} from 'rxjs';

@Component({
  selector: 'app-registration-page',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './registration-page.component.html',
  standalone: true,
  styleUrl: './registration-page.component.scss'
})
export class RegistrationPageComponent {
  registerForm: FormGroup;
  registrationAttempted: boolean = false;
  registrationError: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required, Validators.minLength(6), Validators.maxLength(28)],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        this.strongPasswordValidator
      ]],
      confirmPassword: ['', Validators.required]
    });

    // Add the passwordsMatch validator to confirmPassword
    this.registerForm.get('confirmPassword')?.setValidators([
      Validators.required,
      this.passwordsMatchValidator.bind(this),
    ]);

    // Update confirmPassword validity on password value change
    this.registerForm.get('password')?.valueChanges.subscribe(() => {
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  passwordsMatchValidator(control: any) {
    const password = this.registerForm.get('password')?.value;
    const confirmPassword = control.value;
    return password === confirmPassword ? null : {mismatch: true};
  }

  strongPasswordValidator(control: FormControl): { [key: string]: boolean } | null {
    const value = control.value;

    if (!value) {
      return null; // If the field is empty, leave validation to `Validators.required`
    }

    // Regular expression for a strong password
    const strongPasswordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;

    if (!strongPasswordPattern.test(value)) {
      return {strongPassword: true}; // Return an error object if validation fails
    }

    return null; // Return null if validation passes
  }

  getFormControlErrors(controlName: string): { [key: string]: boolean } | null {
    const control = this.registerForm.get(controlName);
    return control?.errors || null;
  }

  async onSubmit(): Promise<void> {
    this.registrationAttempted = true; // Mark as attempted

    if (this.registerForm.valid) {
      const {username, password} = this.registerForm.value;

      const request: RegisterDto = {username: username, plainPassword: password};

      this.authService.register(request).pipe(
        tap(async (success) => {

          if (success) {
            this.registrationError = false;
            await this.router.navigate(['/login']);
          } else {
            this.registrationError = true;
          }

        }),
        catchError((error) => {
          this.registrationError = true; // Show error message
          console.error('Registration error:', error); // Optionally log the error
          return of(null); // Return an observable to complete the stream
        })
      ).subscribe(); // Subscribe to execute the stream
    } else {
      this.registrationError = true; // Show error message
    }
  }

}
