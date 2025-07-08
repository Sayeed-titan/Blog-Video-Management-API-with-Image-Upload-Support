import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BlogRoutingModule } from './blog-routing.module';
import { BlogFormComponent } from './blog-form/blog-form.component';
import { BlogListComponent } from './blog-list/blog-list.component';
import { RouterModule } from '@angular/router';
import { MatModule } from 'src/app/shared/mat/mat.module';




@NgModule({
  declarations: [
    BlogFormComponent,
    BlogListComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    MatModule,
  ]
})
export class BlogModule { }
