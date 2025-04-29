// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-login',
//   imports: [],
//   templateUrl: './login.component.html',
//   styleUrl: './login.component.css'
// })
// export class LoginComponent {

// }
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router'; 

@Component({
  selector: 'app-login',
  standalone: true, 
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, FormsModule, RouterModule] 
})

export class LoginComponent {
  email: string = '';
  password: string = '';
  constructor(private router: Router) {} 
  // onSubmit() {
    // console.log('Email:', this.email);
    // console.log('Password:', this.password);
    onSubmit() {
      // You can add login validation logic here
      // this.router.navigate(['/landing']); // Navigate to landing page
      if (this.email.trim() && this.password.trim()) {
        // Proceed to landing page
        this.router.navigate(['/landing']);
      } else {
        alert('Please enter both email and password.');
      }
    }
  }
