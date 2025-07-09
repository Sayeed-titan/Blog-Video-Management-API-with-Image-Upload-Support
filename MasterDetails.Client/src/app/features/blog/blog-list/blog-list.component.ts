import { Component, OnInit } from '@angular/core';
import { BlogService } from '../core/services/blog.service';
import { Blog } from 'src/app/models/blog.model';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router'; // Ensure this is imported
import { MatChipsModule } from '@angular/material/chips';
import { PageEvent } from '@angular/material/paginator';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';


@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css']
})
export class BlogListComponent implements OnInit {
  blogs: Blog[] = [];

  pagedBlogs: Blog[] = [];   // only blogs to show on current page

  // pagination config
  totalLength = 0;
  pageSize = 5;
  currentPage = 0;

  apiBaseUrl = environment.apiBaseUrl;

  constructor(private blogService: BlogService,private router: Router, private snackbar: SnackbarService,private dialog: MatDialog) {}

  ngOnInit(): void {
    this.blogService.getAll().subscribe({
      next: (data) => this.blogs = data,
      error: (err) => console.error('Failed to load blogs:', err)
    });
    this.loadBlogs();
  }

    loadBlogs(): void {
    this.blogService.getAll().subscribe({
      next: (data) => {
        this.blogs = data;
        this.totalLength = data.length;
        this.updatePagedBlogs(); // display first page initially
      },
      error: (err) => {
        console.error('Failed to load blogs:', err);
        this.snackbar.show('❌ Failed to load blogs.', 'Close', 4000);
      }
    });
  }

  updatePagedBlogs(): void {
    const start = this.currentPage * this.pageSize;
    const end = start + this.pageSize;
    this.pagedBlogs = this.blogs.slice(start, end);
  }

    onPageChange(event: PageEvent): void {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.updatePagedBlogs();
  }


    editBlog(id: number): void {
    this.router.navigate(['/blogs/edit', id]);
  }

  
deleteBlog(id: number): void {
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    width: '350px',
    data: {
      title: 'Confirm Delete',
      message: 'Are you sure you want to delete this blog?'
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.blogService.delete(id).subscribe({
        next: () => {
          this.blogs = this.blogs.filter(blog => blog.blogID !== id);
          this.updatePagedBlogs();
          this.snackbar.show('✅ Blog deleted successfully.', 'Close');
        },
        error: (err) => {
          console.error('Delete failed:', err);
          this.snackbar.show('❌ Failed to delete blog.', 'Close', 4000);
        }
      });
    }
  });
}




  
}
