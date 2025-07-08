import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/blog/core/services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  constructor(public authService: AuthService, private router: Router) {}

    logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  get userEmail(): string {
  const token = this.authService.accessToken;
  if (!token) return '';
  const payload = JSON.parse(atob(token.split('.')[1]));
  return payload?.email || '';
  }

}
