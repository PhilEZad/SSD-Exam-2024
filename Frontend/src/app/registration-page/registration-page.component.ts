import {Component} from '@angular/core';
import {RegisterDto} from '../../domain/domain';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {AuthService} from '../../services/auth.service';
import {Router, RouterLink} from '@angular/router';

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
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
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
    return password === confirmPassword ? null : { mismatch: true };
  }

  async onSubmit(): Promise<void> {
    this.registrationAttempted = true; // Mark as attempted

    if (this.registerForm.valid) {
      const {username, password} = this.registerForm.value;

      // TODO Prehash password
      const request: RegisterDto = {username: username, plainPassword: password};

      // Call the backend service to register


    } else {
      this.registrationError = true; // Show error message
    }
  }

}
