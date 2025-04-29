import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PortfolioDataService {
  private portfolioData: any;

  setPortfolioData(data: any): void {
    this.portfolioData = data;
  }

  getPortfolioData(): any {
    return this.portfolioData;
  }
}