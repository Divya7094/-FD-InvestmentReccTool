import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
 
@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private router = inject(Router);
 
signupForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/)
    ]],
    confirmPassword: ['', Validators.required]
  }, { validators: this.passwordMatchValidator });
 
  isSubmitting = false;
  isCheckingEmail = false;
  emailExists = false;
  showSuccessModal = false;
 
  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }
 
  async checkEmailExists() {
    const email = this.signupForm.get('email')?.value;
    if (email && this.signupForm.get('email')?.valid) {
      this.isCheckingEmail = true;
      try {
        const response = await this.http.get<any[]>('assets/users.json').toPromise();
this.emailExists = response?.some(user => user.email === email) ?? false;
      } catch (error) {
        console.error('Error checking email:', error);
      } finally {
        this.isCheckingEmail = false;
      }
    }
  }
 
  onSubmit() {
    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }
 
    this.isSubmitting = true;
    const formData = this.signupForm.value;
 
    setTimeout(() => {
      console.log('User registered:', formData);
      this.isSubmitting = false;
      this.showSuccessModal = true;
    }, 1500);
  }
 
  closeModal() {
    this.showSuccessModal = false;
    this.router.navigate(['/login']);
  }
}

