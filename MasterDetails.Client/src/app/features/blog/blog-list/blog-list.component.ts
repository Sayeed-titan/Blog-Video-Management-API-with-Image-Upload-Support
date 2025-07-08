import { Component, OnInit } from '@angular/core';
import { BlogService } from '../core/services/blog.service';
import { Blog } from 'src/app/models/blog.model';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router'; // Ensure this is imported
import { MatChipsModule } from '@angular/material/chips';


@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css']
})
export class BlogListComponent implements OnInit {
  blogs: Blog[] = [];
  apiBaseUrl = environment.apiBaseUrl;

  constructor(private blogService: BlogService,private router: Router) {}

  ngOnInit(): void {
    this.blogService.getAll().subscribe({
      next: (data) => this.blogs = data,
      error: (err) => console.error('Failed to load blogs:', err)
    });
  }

    editBlog(id: number): void {
    this.router.navigate(['/blogs/edit', id]);
  }

  
deleteBlog(id: number): void {
  if (confirm('Are you sure you want to delete this blog?')) {
    this.blogService.delete(id).subscribe({
      next: () => {
        this.blogs = this.blogs.filter(blog => blog.blogID !== id);
        alert('Blog deleted successfully.');
      },
      error: (err) => {
        console.error('Delete failed:', err);
        alert('Failed to delete blog.');
      }
    });
  }
}



  
}
