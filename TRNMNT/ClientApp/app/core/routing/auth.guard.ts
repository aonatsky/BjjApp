import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { tokenNotExpired } from 'angular2-jwt';

import { AuthService } from '../services/auth.service';
import { RouterService } from '../services/router.service'

/** 
 * Decides if a route can be activated. 
 */
@Injectable() export class AuthGuard implements CanActivate {

    constructor(public AuthService: AuthService, private routerService: RouterService) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

        if (this.AuthService.isLoggedIn()) {
            // Signed in.  
            return true;
        }
        // Stores the attempted URL for redirecting.  
        let url: string = state.url;
        this.AuthService.redirectUrl = url;
        // Not signed in so redirects to signin page.  
        this.routerService.GoToLogin(url);
        return false;
    }

}  