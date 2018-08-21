import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '../../../../node_modules/@angular/router';
import { RouterService } from '../../core/services/router.service';

@Component({
  selector: 'app-event-dashboard',
  templateUrl: './event-dashboard.component.html',
  styleUrls: ['./event-dashboard.component.scss']
})
export class EventDashboardComponent implements OnInit {
  constructor(public route: ActivatedRoute, private routerService: RouterService) {}
  eventId: AAGUID;

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
    });
  }
}
