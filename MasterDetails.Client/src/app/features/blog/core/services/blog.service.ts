import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Blog } from 'src/app/models/blog.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private baseUrl = 'http://localhost:5291/api/Blogs';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Blog[]> {
    return this.http.get<Blog[]>(`${this.baseUrl}`);
  }

  getById(id: number): Observable<Blog> {
    return this.http.get<Blog>(`${this.baseUrl}/${id}`);
  }

  create(formData: FormData): Observable<Blog> {
    return this.http.post<Blog>(`${this.baseUrl}/create`, formData);
  }

  update(id: number, formData: FormData): Observable<Blog> {
    return this.http.put<Blog>(`${this.baseUrl}/update/${id}`, formData);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete/${id}`);
  }
}
