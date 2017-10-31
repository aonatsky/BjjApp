import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { tokenNotExpired } from 'angular2-jwt';
import { AuthService } from '../services/auth.service';
import { AuthGuard } from '../routing/auth.guard';
import { RouterService } from '../services/router.service'
import { BrowserDomAdapter } from "@angular/platform-browser/src/browser/browser_adapter";

/** 
 * Decides if a route can be activated. 
 */
@Injectable() export class RedirectGuard implements CanActivate {

    constructor(public AuthService: AuthService, private routerService: RouterService, private authGuard: AuthGuard) { }

    private subdomain: string;
    private dom: BrowserDomAdapter;

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        debugger;
        var homepage = this.getHomePage();
        if (homepage != "") {
            this.routerService.navigateByUrl(homepage);
        }
        else {
            return this.authGuard.canActivate(route, state)
        };
    }

    getHomePage() : string
    {
        return document.getElementById("homepage").innerText;   
    }

}  