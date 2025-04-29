// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-products',
//   imports: [],
//   templateUrl: './products.component.html',
//   styleUrl: './products.component.css'
// })

import { CommonModule } from '@angular/common';
import { Component, NgModule, NgModuleRef } from '@angular/core';
import { FormsModule, NgModel, NgModelGroup } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-products',
  imports: [CommonModule,  RouterLink, FormsModule],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  standalone: true,
  providers: []
})
export class ProductsComponent {
  searchTerm: string = '';
  constructor(private router: Router) {}
  products = [
    { name: 'Growth Equity Fund', type: 'Equity', return: '12%', assetClass: 'equity', description: 'Focused on long-term capital appreciation by investing in high-growth companies.' },
    { name: 'Fixed Income Secure Plan', type: 'Fixed Income', return: '6.5%', assetClass: 'Fixed Income', description: 'Provides regular interest payments in the form of stable returns by investing in government and corporate bonds.' },
    { name: 'Real Estate Investment Trust', type: 'Real Estate', return: '8%', assetClass: 'Real Estate', description: 'Invests in commercial and residential real estate projects across urban areas.' },
    { name: 'Gold & Commodities Basket', type: 'Commodities', return: '7%', assetClass: 'Commodities', description: 'Diversified commodity exposure including gold, oil, and metals for hedging inflation.' },
    { name: 'Ultra-Liquid Fund', type: 'Cash & Cash Equivalents', return: '4%', assetClass: 'Cash and Cash Equivalence', description: 'Designed for short-term liquidity needs with low risk and easy access.' }
  ];

  get filteredProducts() {
    return this.products.filter(product =>
      product.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      product.assetClass.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  getBadgeClass(type: string) {
    switch (type) {
      case 'Equity': return 'bg-success';
      case 'Fixed Income': return 'bg-primary';
      case 'Real Estate': return 'bg-warning text-dark';
      case 'Commodities': return 'bg-secondary';
      case 'Cash & Cash Equivalents': return 'bg-info text-dark';
      default: return 'bg-secondary';
    }
  }
  logout() {
    // Optional: Clear any stored session data here
    this.router.navigate(['']); // navigate to login page
  }
  home() {
    // Optional: Clear any stored session data here
    this.router.navigate(['/landing']); // navigate to login page
  }
}

