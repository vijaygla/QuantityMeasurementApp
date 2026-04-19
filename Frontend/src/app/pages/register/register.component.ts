import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <div class="auth-container">
      <div class="auth-card">
        <h1>Smart Measurement Tool</h1>
        <h2>Create Account</h2>
        <form (ngSubmit)="onRegister()">
          <div class="form-group">
            <label for="name">Full Name</label>
            <input type="text" id="name" name="name" [(ngModel)]="name" required placeholder="John Doe">
          </div>
          <div class="form-group">
            <label for="email">Email</label>
            <input type="email" id="email" name="email" [(ngModel)]="email" required placeholder="your@email.com">
          </div>
          <div class="form-group">
            <label for="password">Password</label>
            <input type="password" id="password" name="password" [(ngModel)]="password" required placeholder="••••••••">
          </div>
          <button type="submit" [disabled]="loading">
            {{ loading ? 'Creating Account...' : 'Register' }}
          </button>
          <p *ngIf="error" class="error-message">{{ error }}</p>
        </form>
        <p class="auth-footer">
          Already have an account? <a routerLink="/login">Login here</a>
        </p>
      </div>
    </div>
  `,
  styles: [`
    .auth-container {
      height: 100vh;
      display: flex;
      justify-content: center;
      align-items: center;
      background-color: var(--bg-color);
    }
    .auth-card {
      background-color: var(--card-bg);
      padding: 2.5rem;
      border-radius: 12px;
      width: 100%;
      max-width: 400px;
      box-shadow: 0 10px 25px rgba(0,0,0,0.5);
      text-align: center;
    }
    h1 { margin-bottom: 0.5rem; font-size: 1.5rem; color: var(--accent-color); }
    h2 { margin-bottom: 2rem; font-weight: 400; color: var(--text-color); }
    .form-group { text-align: left; margin-bottom: 1.5rem; }
    label { display: block; margin-bottom: 0.5rem; color: var(--secondary-color); }
    input { width: 100%; }
    button { 
      width: 100%; 
      padding: 1rem; 
      background-color: var(--accent-color); 
      color: white; 
      font-weight: bold; 
      font-size: 1rem;
      margin-top: 1rem;
    }
    button:hover { opacity: 0.9; }
    button:disabled { background-color: var(--secondary-color); cursor: not-allowed; }
    .auth-footer { margin-top: 1.5rem; color: var(--secondary-color); font-size: 0.9rem; }
  `]
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  loading = false;
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  onRegister() {
    this.loading = true;
    this.error = '';
    this.authService.register({ name: this.name, email: this.email, password: this.password }).subscribe({
      next: (res) => {
        alert('Registration successful! Please login.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Registration failed.';
        this.loading = false;
      }
    });
  }
}
