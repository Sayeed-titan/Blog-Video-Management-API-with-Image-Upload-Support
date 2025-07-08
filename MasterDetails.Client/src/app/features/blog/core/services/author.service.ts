import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Author } from 'src/app/models/author.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private baseUrl = 'http://localhost:5291/api/Authors';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Author[]> {
    return this.http.get<Author[]>(this.baseUrl);
  }
}
