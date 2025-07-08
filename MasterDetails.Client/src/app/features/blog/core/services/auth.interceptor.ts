import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = this.authService.accessToken;

    let authReq = req;
    if (accessToken) {
      authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${accessToken}` }
      });
    }

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && this.authService.refreshToken) {
          return this.authService.refreshTokens().pipe(
            switchMap(() => {
              const newToken = this.authService.accessToken;
              const newReq = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` }
              });
              return next.handle(newReq);
            })
          );
        }
        return throwError(() => error);
      })
    );
  }
}
