import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { tokenNotExpired } from 'angular2-jwt';

import { AuthService } from '../services/auth.service';
import { AuthGuard } from '../routing/auth.guard';
import { RouterService } from '../services/router.service'

/** 
 * Decides if a route can be activated. 
 */
@Injectable() export class RedirectGuard implements CanActivate {

    constructor(public AuthService: AuthService, private routerService: RouterService, private authGuard: AuthGuard) { }

    private subdomain: string;

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        debugger;
        this.getSubdomain();
        if (this.subdomain && this.subdomain != "") {
            this.routerService.GoToEventInfo(this.subdomain);
        }
        else {
            return this.authGuard.canActivate(route, state)
        };
    }

    getSubdomain() {
        const domain = window.location.hostname;
        if (domain.indexOf('.') < 0 ||
            domain.split('.')[0] === 'example' || domain.split('.')[0] === 'lvh' || domain.split('.')[0] === 'www') {
            this.subdomain = '';
        } else {
            this.subdomain = domain.split('.')[0];
        }
        console.log('subdomain', this.subdomain);
    }


}  