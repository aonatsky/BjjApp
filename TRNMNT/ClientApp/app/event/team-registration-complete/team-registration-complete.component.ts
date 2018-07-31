import { Component, OnInit } from '@angular/core';
import { RouterService } from '../../core/services/router.service';

@Component({
  selector: 'app-team-registration-complete',
  templateUrl: './team-registration-complete.component.html'
})
export class TeamRegistrationCompleteComponent implements OnInit {
  constructor(private routerService: RouterService) {}

  ngOnInit() {}

  goToEventPage() {
    this.routerService.goToEventInfo();
  }
}
