import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Tag } from 'src/app/models/tag.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private baseUrl = 'http://localhost:5291/api/Tags';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.baseUrl);
  }

  create(tag: { name: string }): Observable<Tag> {
    return this.http.post<Tag>(this.baseUrl, tag);
  }


}
