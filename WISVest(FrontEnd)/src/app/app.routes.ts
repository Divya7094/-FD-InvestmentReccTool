import { SignupComponent } from './signup/signup.component';
import { Router, Routes } from '@angular/router';
import { PortfolioComponent } from './portfolio/portfolio.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ProductsComponent } from './products/products.component';
import { LoginComponent } from './login/login.component';
import { InvestmentFormComponent } from './investment-form/investment-form.component';
 
export const routes: Routes = [
  { path: 'signup', component: SignupComponent },
  { path: '', redirectTo: 'signup', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'landing', component: LandingPageComponent },
    { path: 'input', component: InvestmentFormComponent },
    {path:'products', component:ProductsComponent},
  { path: 'portfolio', component: PortfolioComponent }
  // other routes...
];
