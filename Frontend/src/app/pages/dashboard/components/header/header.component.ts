import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../services/auth.service';
import { ActivityService } from '../../../../services/activity.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <header>
      <div class="logo-section">
        <span class="logo-text">Quantity Measurement</span>
      </div>
      <div class="title-section">
        <h1>Smart Measurement Tool</h1>
        <p>Convert, Compare and Calculate quantities with precision.</p>
      </div>
      <div class="user-section">
        <div class="profile-container" (click)="toggleDropdown()">
          <div class="profile-icon">
            <svg viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 3c1.66 0 3 1.34 3 3s-1.34 3-3 3-3-1.34-3-3 1.34-3 3-3zm0 14.2c-2.5 0-4.71-1.28-6-3.22.03-1.99 4-3.08 6-3.08 1.99 0 5.97 1.09 6 3.08-1.29 1.94-3.5 3.22-6 3.22z"/>
            </svg>
          </div>
          <span>Profile</span>
          
          <div class="dropdown" *ngIf="showDropdown()" (click)="$event.stopPropagation()">
            <div class="user-info">
              <strong>{{ authService.currentUser() }}</strong>
              <button (click)="authService.logout()" class="logout-btn">Logout</button>
            </div>
            <div class="activity-history">
              <h3>Recent Activity</h3>
              <div class="history-list">
                <div *ngFor="let item of activityService.getActivities()()" class="history-item">
                  <div class="history-time">{{ item.timestamp | date:'short' }}</div>
                  <div class="history-desc">{{ item.operation }} in {{ item.category }}</div>
                  <div class="history-detail">{{ item.input }} = <strong>{{ item.result }}</strong></div>
                </div>
                <div *ngIf="activityService.getActivities()().length === 0" class="no-activity">
                  No recent activity
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>
  `,
  styles: [`
    header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 2rem;
      background-color: var(--bg-color);
      border-bottom: 1px solid var(--border-color);
    }
    .logo-text { font-weight: bold; font-size: 1.2rem; }
    .title-section { text-align: center; }
    .title-section h1 { font-size: 1.8rem; color: var(--accent-color); margin-bottom: 0.25rem; }
    .title-section p { color: var(--secondary-color); font-size: 0.9rem; }
    .user-section { position: relative; }
    .profile-container {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      cursor: pointer;
      padding: 0.5rem;
      border-radius: 4px;
      transition: background 0.2s;
    }
    .profile-container:hover { background: var(--hover-bg); }
    .profile-icon { width: 32px; height: 32px; color: var(--accent-color); }
    
    .dropdown {
      position: absolute;
      top: 100%;
      right: 0;
      width: 300px;
      background: var(--card-bg);
      border: 1px solid var(--border-color);
      border-radius: 8px;
      margin-top: 0.5rem;
      box-shadow: 0 10px 25px rgba(0,0,0,0.5);
      z-index: 100;
      padding: 1rem;
    }
    .user-info {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-bottom: 1rem;
      border-bottom: 1px solid var(--border-color);
      margin-bottom: 1rem;
    }
    .logout-btn {
      padding: 0.4rem 0.8rem;
      background: #f87171;
      color: white;
      font-size: 0.8rem;
    }
    .activity-history h3 { font-size: 0.9rem; margin-bottom: 0.75rem; color: var(--secondary-color); }
    .history-list { max-height: 250px; overflow-y: auto; }
    .history-item {
      padding: 0.5rem;
      border-bottom: 1px solid var(--border-color);
      font-size: 0.8rem;
    }
    .history-time { font-size: 0.7rem; color: var(--secondary-color); }
    .history-desc { margin: 0.2rem 0; font-weight: bold; }
    .history-detail { color: var(--accent-color); }
    .no-activity { text-align: center; padding: 1rem; color: var(--secondary-color); font-size: 0.8rem; }
  `]
})
export class HeaderComponent {
  showDropdown = signal(false);

  constructor(public authService: AuthService, public activityService: ActivityService) {}

  toggleDropdown() {
    this.showDropdown.set(!this.showDropdown());
  }
}
