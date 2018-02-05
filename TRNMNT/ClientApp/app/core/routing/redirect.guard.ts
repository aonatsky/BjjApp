import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AuthGuard } from '../routing/auth.guard';
import { RouterService } from '../services/router.service'
import { BrowserDomAdapter } from "@angular/platform-browser/src/browser/browser_adapter";

/** 
 * Decides if a route can be activated. 
 */
@Injectable() export class RedirectGuard implements CanActivate {

    constructor(public authService: AuthService, private routerService: RouterService, private authGuard: AuthGuard) { }

    private subdomain: string;
    private dom: BrowserDomAdapter;

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        var homepage = this.getHomePage();
        if (homepage != "") {
            this.routerService.navigateByUrl(homepage);
        }
        else {
            return this.authGuard.canActivate(route, state);
        };
    }

    getHomePage() : string
    {
        return document.getElementById("homepage").innerText;   
    }

}  