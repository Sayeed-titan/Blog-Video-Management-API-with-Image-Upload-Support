import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Blog } from 'src/app/models/blog.model';
import { BlogService } from '../core/services/blog.service';
import { TagService } from '../core/services/tag.service';
import { Tag } from 'src/app/models/tag.model';

@Component({
  selector: 'app-blog-form',
  templateUrl: './blog-form.component.html',
  styleUrls: ['./blog-form.component.scss']
})
export class BlogFormComponent implements OnInit {
  blogForm!: FormGroup;
  coverImageFile?: File;

  isEditMode = false;
  blogId!: number;

   tags: Tag[] = [];

  constructor(
    private fb: FormBuilder,
    private blogService: BlogService,
    private tagService: TagService, 

    private route: ActivatedRoute,
    private router: Router
  ) {}

ngOnInit(): void {
  this.initForm();

  this.tagService.getAll().subscribe({
    next: (tags) => this.tags = tags,
    error: (err) => console.error('Failed to load tags:', err)
  });

  const id = this.route.snapshot.paramMap.get('id');
  if (id) {
    this.isEditMode = true;
    this.blogId = +id;
    this.blogService.getById(this.blogId).subscribe({
      next: (blog) => this.patchForm(blog),
      error: (err) => console.error('Failed to load blog:', err)
    });
  }
}


  initForm(): void {
    this.blogForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      authorName: ['', Validators.required],
      createdAt: [new Date(), Validators.required],
      tagNames: [[]], // array of string names
      isPublished: [false],
      coverImage: [null],
      blogVideos: this.fb.array([])
    });
  }

  get blogVideos(): FormArray {
    return this.blogForm.get('blogVideos') as FormArray;
  }

  addVideo(): void {
    this.blogVideos.push(
      this.fb.group({
        videoUrl: ['', Validators.required],
        caption: [''],
        displayOrder: [1]
      })
    );
  }

  removeVideo(index: number): void {
    this.blogVideos.removeAt(index);
  }

  onCoverImageChange(event: any): void {
    const file = event.target.files[0];
    if (file) this.coverImageFile = file;
  }

  patchForm(blog: Blog): void {
    this.blogForm.patchValue({
      title: blog.title,
      content: blog.content,
      authorName: blog.authorName, // authorName is just a string
      tagNames: blog.tagNames,
      isPublished: blog.isPublished
    });

    const videoControls = blog.blogVideos.map(video =>
      this.fb.group({
        videoUrl: [video.videoUrl],
        caption: [video.caption],
        displayOrder: [video.displayOrder || 0]
      })
    );

    this.blogForm.setControl('blogVideos', this.fb.array(videoControls));
  }

  submit(): void {
    const formData = new FormData();

    formData.append('title', this.blogForm.value.title);
    formData.append('content', this.blogForm.value.content);
    formData.append('authorName', this.blogForm.value.authorName);
    formData.append('isPublished', this.blogForm.value.isPublished.toString());

    this.blogForm.value.tagNames.forEach((tagName: string) => {
      formData.append('tagNames', tagName);
    });

    formData.append('blogVideos', JSON.stringify(this.blogForm.value.blogVideos));

    if (this.coverImageFile) {
      formData.append('coverImage', this.coverImageFile, this.coverImageFile.name);
    }

    if (this.isEditMode) {
      this.blogService.update(this.blogId, formData).subscribe({
        next: () => {
          alert('Blog updated!');
          this.router.navigate(['/blogs']);
        },
        error: (err) => console.error('Update failed:', err)
      });
    } else {
      this.blogService.create(formData).subscribe({
        next: () => {
          alert('Blog created!');
          this.router.navigate(['/blogs']);
        },
        error: (err) => console.error('Creation failed:', err)
      });
    }
  }
}
