import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly API_URL = '/api/auth';
  
  private userEmail = signal<string | null>(null);
  private token = signal<string | null>(null);

  isAuthenticated = computed(() => !!this.token());
  currentUser = computed(() => this.userEmail());

  constructor(private http: HttpClient, private router: Router) {
    this.loadToken();
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.API_URL}/register`, request);
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.API_URL}/login`, request).pipe(
      tap(res => {
        if (res.token) {
          this.saveToken(res.token, request.email);
        }
      })
    );
  }

  logout() {
    this.token.set(null);
    this.userEmail.set(null);
    if (typeof window !== 'undefined') {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem('user_email');
    }
    this.router.navigate(['/login']);
  }

  getToken() {
    return this.token();
  }

  private saveToken(token: string, email: string) {
    this.token.set(token);
    this.userEmail.set(email);
    if (typeof window !== 'undefined') {
      localStorage.setItem(this.TOKEN_KEY, token);
      localStorage.setItem('user_email', email);
    }
  }

  private loadToken() {
    if (typeof window !== 'undefined') {
      const storedToken = localStorage.getItem(this.TOKEN_KEY);
      const storedEmail = localStorage.getItem('user_email');
      if (storedToken) {
        this.token.set(storedToken);
        this.userEmail.set(storedEmail);
      }
    }
  }
}
