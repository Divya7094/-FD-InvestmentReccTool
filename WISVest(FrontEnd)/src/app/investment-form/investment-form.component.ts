import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserInputService, UserInput } from '../services/user-input.service';
import { CommonModule } from '@angular/common';
import { PortfolioDataService } from '../services/portfoliodata.service';

@Component({
  selector: 'app-investment-form',
  standalone:true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './investment-form.component.html',
  styleUrls: ['./investment-form.component.css']
})
export class InvestmentFormComponent implements OnInit {
  investmentForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userInputService: UserInputService,
    private portfolioDataService: PortfolioDataService
  ) {}

  ngOnInit(): void {
    this.investmentForm = this.fb.group({
      goal: ['', Validators.required],
      age: ['', [Validators.required, Validators.min(18), Validators.max(100)]],
      targetAmount: ['', [Validators.required, Validators.min(10000)]],
      riskTolerance: ['', Validators.required],
      investmentHorizon: ['', Validators.required],
    });
  }

  selectHorizon(horizon: string): void {
    this.investmentForm.get('investmentHorizon')?.setValue(horizon);
  }

  selectRisk(risk: string): void {
    this.investmentForm.get('riskTolerance')?.setValue(risk);
  }

  onSubmit(): void {
    if (this.investmentForm.valid) {
      const userInput: UserInput = this.investmentForm.value;
      this.userInputService.submitUserInput(userInput).subscribe({
        next: (response) => {
          console.log('Submission successful:', response);
  
          const portfolioData = {
            totalAmount: userInput.targetAmount,
            allocatedMatrix: [
              { assetClass: 'Equity', percentage: response.equity, color: '#FF6384' },
              { assetClass: 'Fixed Income', percentage: response.fixedIncome, color: '#36A2EB' },
              { assetClass: 'Commodities', percentage: response.commodities, color: '#FFCE56' },
              { assetClass: 'Cash', percentage: response.cash, color: '#4BC0C0' },
              { assetClass: 'Real Estate', percentage: response.realEstate, color: '#9966FF' }
            ]
          };
  
          // Store the portfolio data in the service
          this.portfolioDataService.setPortfolioData(portfolioData);
  
          console.log('Navigating to portfolio with state:', { portfolioData });
          this.router.navigate(['/portfolio']);
        },
        error: (error) => {
          console.error('Submission failed:', error);
        }
      });
    } else {
      console.log('Form is invalid');
      this.investmentForm.markAllAsTouched();
    }
  }

  home(): void {
    this.router.navigate(['/landing']);
  }

  products(): void {
    this.router.navigate(['/products']);
  }

  logout(): void {
    this.router.navigate(['/login']);
  }
  speakText(text: string): void {
    // Stop any ongoing speech
    if (window.speechSynthesis.speaking) {
      window.speechSynthesis.cancel();
    }
  
    // Create a new speech utterance
    const speech = new SpeechSynthesisUtterance(text);
    speech.lang = 'en-US'; // Set the language
    speech.pitch = 1; // Set the pitch
    speech.rate = 1; // Set the rate of speech
    window.speechSynthesis.speak(speech);
  }
}