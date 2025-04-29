// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-landing-page',
//   imports: [],
//   templateUrl: './landing-page.component.html',
//   styleUrl: './landing-page.component.css'
// })
// export class LandingPageComponent {

// }
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  standalone:true,
  imports: [],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {
  constructor(private router: Router) {}

  logout() {
    // Optional: Clear any stored session data here
    this.router.navigate(['/login']); // navigate to login page
  }
  products() {
    // Optional: Clear any stored session data here
    this.router.navigate(['/products']); // navigate to login page
  }
  input() {
    // Optional: Clear any stored session data here
    this.router.navigate(['/input']); // navigate to login page
  }

}
