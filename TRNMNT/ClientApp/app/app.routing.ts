import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginComponent } from './shared/login/login.component';
import { RegisterComponent } from './shared/register/register.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'event-admin', pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, pathMatch: 'full'},
  { path: 'register', component: RegisterComponent, pathMatch: 'full' },
  { path: 'event', redirectTo: 'event' },
  { path: 'event-admin', redirectTo: 'event-admin'},
  { path: '**', redirectTo: 'event-admin'}
];
