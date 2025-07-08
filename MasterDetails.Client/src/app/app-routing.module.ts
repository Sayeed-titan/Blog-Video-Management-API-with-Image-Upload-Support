import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlogFormComponent } from './features/blog/blog-form/blog-form.component';
import { BlogModule } from './features/blog/blog.module';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: 'blogs', pathMatch: 'full' },
  {
    path: 'blogs',
    loadChildren: () =>
      import('./features/blog/blog.module').then((m) => m.BlogModule),
  },
    { path: 'blogs/edit/:id', component: BlogFormComponent, canActivate: [AuthGuard] }

];


@NgModule({
  imports: [RouterModule.forRoot(routes), BlogModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
