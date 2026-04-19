import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MeasurementService } from '../../../../services/measurement.service';
import { ActivityService } from '../../../../services/activity.service';
import { SharedStateService } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-calculator',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="calculator-container">
      <div class="operations-tabs">
        <button 
          *ngFor="let op of operations" 
          [class.active]="selectedOperation() === op"
          (click)="selectedOperation.set(op)">
          {{ op }}
        </button>
      </div>

      <div class="calculator-card">
        <div class="input-grid">
          <div class="input-section">
            <label>Primary Value</label>
            <input type="number" [(ngModel)]="firstValue" placeholder="0">
            <select [(ngModel)]="firstUnit">
              <option *ngFor="let unit of currentUnits()" [value]="unit">{{ unit }}</option>
            </select>
          </div>

          <div class="divider-icon" *ngIf="selectedOperation() !== 'Convert'">
            <span>{{ getOpIcon() }}</span>
          </div>

          <div class="input-section">
            <label>{{ selectedOperation() === 'Convert' ? 'Target Unit' : 'Secondary Value' }}</label>
            <ng-container *ngIf="selectedOperation() !== 'Convert'">
              <input type="number" [(ngModel)]="secondValue" placeholder="0">
              <select [(ngModel)]="secondUnit">
                <option *ngFor="let unit of currentUnits()" [value]="unit">{{ unit }}</option>
              </select>
            </ng-container>
            <ng-container *ngIf="selectedOperation() === 'Convert'">
              <select [(ngModel)]="targetUnit" class="full-width">
                <option *ngFor="let unit of currentUnits()" [value]="unit">{{ unit }}</option>
              </select>
            </ng-container>
          </div>
        </div>

        <div class="math-target" *ngIf="isMathOp()">
           <label>Result Unit (Optional)</label>
           <select [(ngModel)]="targetUnit">
             <option value="">Auto</option>
             <option *ngFor="let unit of currentUnits()" [value]="unit">{{ unit }}</option>
           </select>
        </div>

        <button class="calculate-btn" (click)="onCalculate()" [disabled]="loading()">
          <span class="icon">📊</span>
          {{ loading() ? 'Calculating...' : 'Calculate & Save' }}
        </button>

        <div class="result-display" *ngIf="result()">
          <div class="result-label">Result:</div>
          <div class="result-value">{{ result() }}</div>
        </div>
        <p *ngIf="error()" class="error-message">{{ error() }}</p>
      </div>
    </div>
  `,
  styles: [`
    .calculator-container {
      flex: 1;
      padding: 2rem;
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
      overflow-y: auto;
    }
    .operations-tabs {
      display: flex;
      background: var(--card-bg);
      padding: 0.5rem;
      border-radius: 8px;
      gap: 0.5rem;
    }
    .operations-tabs button {
      flex: 1;
      padding: 0.75rem;
      background: transparent;
      color: var(--secondary-color);
      font-weight: bold;
    }
    .operations-tabs button.active {
      background: var(--tab-active);
      color: white;
      border-radius: 6px;
    }
    .calculator-card {
      background: var(--card-bg);
      padding: 2rem;
      border-radius: 12px;
      box-shadow: 0 4px 6px rgba(0,0,0,0.1);
    }
    .input-grid {
      display: grid;
      grid-template-columns: 1fr auto 1fr;
      gap: 2rem;
      align-items: center;
      margin-bottom: 2rem;
    }
    @media (max-width: 600px) {
      .input-grid { grid-template-columns: 1fr; }
      .divider-icon { transform: rotate(90deg); }
    }
    .input-section { display: flex; flex-direction: column; gap: 0.75rem; }
    .input-section label { font-size: 0.8rem; color: var(--secondary-color); }
    .divider-icon { font-size: 1.5rem; color: var(--accent-color); }
    .full-width { width: 100%; margin-top: 1.6rem; }
    .math-target { margin-bottom: 2rem; display: flex; flex-direction: column; gap: 0.5rem; }
    .math-target label { font-size: 0.8rem; color: var(--secondary-color); }
    .calculate-btn {
      width: 100%;
      padding: 1rem;
      background: var(--accent-color);
      color: white;
      font-weight: bold;
      font-size: 1.1rem;
      display: flex;
      justify-content: center;
      align-items: center;
      gap: 0.5rem;
    }
    .calculate-btn:hover { filter: brightness(1.1); }
    .result-display {
      margin-top: 2rem;
      padding: 1.5rem;
      background: var(--input-bg);
      border-left: 4px solid var(--accent-color);
      border-radius: 4px;
    }
    .result-label { font-size: 0.8rem; color: var(--secondary-color); margin-bottom: 0.5rem; }
    .result-value { font-size: 1.5rem; font-weight: bold; color: var(--accent-color); }
  `]
})
export class CalculatorComponent {
  operations = ['Compare', 'Convert', 'Add', 'Subtract', 'Divide', 'Multiply'];
  selectedOperation = signal('Compare');
  
  firstValue = 0;
  firstUnit = '';
  secondValue = 0;
  secondUnit = '';
  targetUnit = '';

  loading = signal(false);
  result = signal<string | null>(null);
  error = signal<string | null>(null);

  currentUnits = computed(() => {
    const units = this.state.units();
    const cat = this.state.selectedCategory();
    const list = units[cat] || [];
    if (list.length > 0 && (!this.firstUnit || !list.includes(this.firstUnit))) {
        this.firstUnit = list[0];
        this.secondUnit = list[0];
        this.targetUnit = list[0];
    }
    return list;
  });

  constructor(
    private state: SharedStateService, 
    private measurementService: MeasurementService,
    private activityService: ActivityService
  ) {}

  isMathOp() {
    return ['Add', 'Subtract'].includes(this.selectedOperation());
  }

  getOpIcon() {
    switch(this.selectedOperation()) {
      case 'Compare': return '⚖️';
      case 'Add': return '➕';
      case 'Subtract': return '➖';
      case 'Divide': return '➗';
      case 'Multiply': return '✖️';
      default: return '➡️';
    }
  }

  onCalculate() {
    this.loading.set(true);
    this.result.set(null);
    this.error.set(null);

    const first = { value: this.firstValue, unit: this.firstUnit };
    const second = { value: this.secondValue, unit: this.secondUnit };
    const op = this.selectedOperation();
    const cat = this.state.selectedCategory();

    let obs: any;
    let logInput = '';

    switch(op) {
      case 'Compare':
        obs = this.measurementService.compare({ first, second });
        logInput = `${this.firstValue} ${this.firstUnit} vs ${this.secondValue} ${this.secondUnit}`;
        break;
      case 'Convert':
        obs = this.measurementService.convert({ value: this.firstValue, unit: this.firstUnit, targetUnit: this.targetUnit });
        logInput = `${this.firstValue} ${this.firstUnit} to ${this.targetUnit}`;
        break;
      case 'Add':
        obs = this.measurementService.add({ first, second, targetUnit: this.targetUnit || undefined });
        logInput = `${this.firstValue} ${this.firstUnit} + ${this.secondValue} ${this.secondUnit}`;
        break;
      case 'Subtract':
        obs = this.measurementService.subtract({ first, second, targetUnit: this.targetUnit || undefined });
        logInput = `${this.firstValue} ${this.firstUnit} - ${this.secondValue} ${this.secondUnit}`;
        break;
      case 'Divide':
        obs = this.measurementService.divide({ first, second });
        logInput = `${this.firstValue} ${this.firstUnit} / ${this.secondValue} ${this.secondUnit}`;
        break;
      case 'Multiply':
        obs = this.measurementService.multiply({ first, second });
        logInput = `${this.firstValue} ${this.firstUnit} * ${this.secondValue} ${this.secondUnit}`;
        break;
    }

    if (obs) {
      obs.subscribe({
        next: (res: any) => {
          let resStr = '';
          if (op === 'Compare') resStr = res.message;
          else if (op === 'Divide') resStr = `Ratio: ${res.ratio}`;
          else if (op === 'Multiply') resStr = `Product: ${res.productOfBaseValues}`;
          else resStr = `${res.value} ${res.unit}`;

          this.result.set(resStr);
          this.activityService.addActivity({
            operation: op,
            category: cat,
            input: logInput,
            result: resStr
          });
          this.loading.set(false);
        },
        error: (err: any) => {
          this.error.set(err.error?.message || 'Operation failed. Make sure the units are compatible.');
          this.loading.set(false);
        }
      });
    }
  }
}
