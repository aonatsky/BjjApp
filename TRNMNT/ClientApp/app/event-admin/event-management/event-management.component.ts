import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'event-management',
  templateUrl: './event-management.component.html'
})
export class EventManagementComponent implements OnInit {
  eventId: string;
  page: string;
  activeTabIndex: number = 0;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
    });
    this.route.queryParams.subscribe(p => {
      switch (p.page) {
        case 'brackets': {
          this.activeTabIndex = 1;
          break;
        }
        case 'participants': {
          this.activeTabIndex = 0;
          break;
        }
        case 'results': {
          this.activeTabIndex = 2;
          break;
        }
        default:{
          this.activeTabIndex = 0
        }
      }
    });
  }
}
