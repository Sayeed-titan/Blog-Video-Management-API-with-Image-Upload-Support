import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = 'http://localhost:5291/api/auth'; // Updated this
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient) {}

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.api}/login`, data).pipe(
      tap((res: any) => {
        localStorage.setItem('accessToken', res.accessToken);
        localStorage.setItem('refreshToken', res.refreshToken);
        this.isAuthenticatedSubject.next(true);
      })
    );
  }

  register(data: any): Observable<any> {
    return this.http.post(`${this.api}/register`, data);
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.isAuthenticatedSubject.next(false);
  }

  get accessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  get refreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  refreshTokens(): Observable<any> {
    const userId = this.getUserIdFromToken(); // implement if needed
    return this.http.post(`${this.api}/refresh-token`, {
      userId,
      refreshToken: this.refreshToken
    }).pipe(
      tap((res: any) => {
        localStorage.setItem('accessToken', res.accessToken);
        localStorage.setItem('refreshToken', res.refreshToken);
      })
    );
  }

  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  private getUserIdFromToken(): number {
    const token = this.accessToken;
    if (!token) return 0;
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.nameid || payload?.sub || 0;
  }
}
