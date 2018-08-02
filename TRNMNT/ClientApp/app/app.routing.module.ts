import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './shared/register/register.component';
import { LoginPageComponent } from './shared/login/login-page.component';
import { TopbarComponent } from './shared/topbar/topbar.component';
import { FooterComponent } from './shared/footer/footer.component';

const appRoutes: Routes = [
  {
    path: '',
    redirectTo: 'event-admin',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginPageComponent,
    pathMatch: 'full'
  },
  {
    path: 'register',
    component: RegisterComponent,
    pathMatch: 'full'
  },
  { path: '**', redirectTo: 'event-admin' },
  {
    path: '',
    outlet: 'topmenu',
    component: TopbarComponent
  },
  {
    path: '',
    outlet: 'footer',
    component: FooterComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, { enableTracing: false })],
  providers: [RouterModule]
})
export class AppRoutingModule {}
