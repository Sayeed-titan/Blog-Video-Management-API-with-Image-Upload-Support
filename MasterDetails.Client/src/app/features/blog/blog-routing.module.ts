import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlogListComponent } from './blog-list/blog-list.component';
import { BlogFormComponent } from './blog-form/blog-form.component';
import { AuthGuard } from 'src/app/auth/auth.guard';


const routes: Routes = [
  { path: '', component: BlogListComponent },
  { path: 'create', component: BlogFormComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule {}
