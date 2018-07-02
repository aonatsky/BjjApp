import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { RedirectGuard } from './redirect.guard';
import { LoginComponent } from '../../shared/login/login.component';
import { RegisterComponent } from '../../shared/register/register.component';


export const appRoutes: Routes = [
    { path: '', redirectTo: 'event-admin', pathMatch: 'full', canActivate: [RedirectGuard, AuthGuard] },
    { path: 'login', component: LoginComponent, pathMatch: 'full' },
    { path: 'register', component: RegisterComponent, pathMatch: 'full' },
    { path: 'event', redirectTo: 'event' },
    { path: 'home', redirectTo: 'event-admin' },
    { path: '**', redirectTo: 'event-admin', canActivate: [AuthGuard] }
];




