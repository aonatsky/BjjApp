import { Location } from '@angular/common';
import { Injectable, Inject } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { NavigationExtras } from '@angular/router';

@Injectable()
export class RouterService {
  constructor(private router: Router, private location: Location, @Inject(Window) private win: Window) {}

  navigationStartEvents(): Observable<NavigationStart> {
    return this.router.events.pipe(filter(event => event instanceof NavigationStart)) as Observable<NavigationStart>;
  }

  navigateByUrl(url: string, options?: NavigationExtras) {
    this.router.navigateByUrl(url, options);
  }

  openNewWindow(url: string) {
    this.win.open(url);
  }

  goToMainDomain(subdomain: string = '') {
    if (subdomain !== '') {
      const path = location.host.replace(subdomain + '.', '');
      location.href = location.protocol + '//' + path;
    }
  }

  goHome(){
    
    if(this.isEventPortal()){
      this.goToEventInfo()
    }else{
      this.goToEventAdmin()
    }
  }

  goToLogin(returnUrl?: string) {
    this.router.navigate(['/login'], { queryParams: { returnUrl } });
  }

  goToRegistration(returnUrl?: string) {
    this.router.navigate(['/register'], { queryParams: { returnUrl } });
  }

  goToOrganizerScreen() {
    this.router.navigateByUrl('/administration/home');
  }

  goToEventAdmin() {
    this.router.navigateByUrl('/event-admin');
  }

  goToEditEvent(id: string) {
    this.router.navigateByUrl(`/event-admin/edit/${id}`);
  }

  goToEventManagement(id: string) {
    this.router.navigateByUrl(`/event-admin/management/${id}`);
  }

  goToEventRun(id: string) {
    this.router.navigateByUrl(`/event-admin/run/${id}`);
  }

  goToEventInfo() {
    this.router.navigateByUrl('/event');
  }

  goToParticipantRegistration() {
    this.router.navigateByUrl('/event/participant-registration');
  }

  goToMyProfile(returnUrl? : string) {
    this.router.navigateByUrl('/profile',{ queryParams: { returnUrl } });
  }
  
  goToParticipantTeamRegistration() {
    this.router.navigateByUrl('/event/participant-team-registration');
  }

  goToTeamRegistration(returnUrl? : string) {
    this.router.navigateByUrl('/event/team-registration',{ queryParams: { returnUrl } });
  }

  goToTeamList(){
    this.router.navigateByUrl('/event-admin/teams');
  }

  openEventWeightDivisionSpectatorView() {
    this.openNewWindow('/event-admin/run-wd-spectator-view');
  }

  openEventCategorySpectatorView(categoryId: string) {
    this.openNewWindow(`/event-admin/run-category-spectator-view/${categoryId}`);
  }

  getCurrentUrl() {
    return this.location.path();
  }

  getSubdomains(): string[] {
    const parts = window.location.host.split('.');
    parts.pop();
    if (parts.filter(p => p.indexOf('trnmnt') !== -1).length > 0) {
      parts.pop();
    }
    return parts;
  }

  getMainDomainUrl() {
    var subdomains = this.getSubdomains();
    let path = location.host;
    for (const subdomain of subdomains) {
      path = path.replace(subdomain + '.', '');
    }
    return location.protocol + '//' + path;
  }

  isEventPortal() : boolean{
    return this.getSubdomains().length > 0;
  }
}
