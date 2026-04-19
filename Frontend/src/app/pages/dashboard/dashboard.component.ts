import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { CalculatorComponent } from './components/calculator/calculator.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, HeaderComponent, SidebarComponent, CalculatorComponent],
  template: `
    <div class="dashboard-layout">
      <app-header></app-header>
      <div class="main-content">
        <app-sidebar></app-sidebar>
        <app-calculator></app-calculator>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-layout {
      display: flex;
      flex-direction: column;
      height: 100vh;
    }
    .main-content {
      display: flex;
      flex: 1;
      overflow: hidden;
    }
    @media (max-width: 768px) {
      .main-content {
        flex-direction: column;
        overflow-y: auto;
      }
    }
  `]
})
export class DashboardComponent {}
