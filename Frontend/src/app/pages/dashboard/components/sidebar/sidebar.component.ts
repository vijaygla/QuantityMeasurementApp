import { Component, OnInit, signal, effect, Injectable } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MeasurementService } from '../../../../services/measurement.service';

@Injectable({ providedIn: 'root' })
export class SharedStateService {
  selectedCategory = signal<string>('Length');
  units = signal<Record<string, string[]>>({});
  
  setCategory(cat: string) {
    this.selectedCategory.set(cat);
  }
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <aside>
      <h3>Measurement Category</h3>
      <div class="category-list">
        <button 
          *ngFor="let cat of categories" 
          [class.active]="state.selectedCategory() === cat"
          (click)="selectCategory(cat)">
          <span class="icon">{{ getIcon(cat) }}</span>
          {{ cat }}
        </button>
      </div>
    </aside>
  `,
  styles: [`
    aside {
      width: 250px;
      padding: 1.5rem;
      background-color: var(--card-bg);
      border-right: 1px solid var(--border-color);
    }
    h3 { font-size: 0.9rem; margin-bottom: 1.5rem; color: var(--secondary-color); text-transform: uppercase; }
    .category-list { display: flex; flex-direction: column; gap: 0.75rem; }
    button {
      display: flex;
      align-items: center;
      padding: 1rem;
      background: var(--tab-inactive);
      color: var(--text-color);
      text-align: left;
      border-radius: 8px;
    }
    button:hover { background: var(--hover-bg); }
    button.active { background: var(--tab-active); color: white; }
    .icon { margin-right: 0.75rem; font-size: 1.2rem; }
    @media (max-width: 768px) {
      aside { width: 100%; border-right: none; border-bottom: 1px solid var(--border-color); }
      .category-list { flex-direction: row; flex-wrap: wrap; }
      button { flex: 1; min-width: 120px; }
    }
  `]
})
export class SidebarComponent implements OnInit {
  categories: string[] = ['Length', 'Weight', 'Temperature', 'Volume'];

  constructor(public state: SharedStateService, private measurementService: MeasurementService) {}

  ngOnInit() {
    this.measurementService.getUnits().subscribe(units => {
      this.state.units.set(units);
      const keys = Object.keys(units);
      if (keys.length > 0) {
        this.categories = keys;
      }
    });
  }

  selectCategory(cat: string) {
    this.state.setCategory(cat);
  }

  getIcon(cat: string) {
    switch(cat) {
      case 'Length': return '📏';
      case 'Weight': return '⚖️';
      case 'Temperature': return '🌡️';
      case 'Volume': return '🧪';
      default: return '📦';
    }
  }
}
