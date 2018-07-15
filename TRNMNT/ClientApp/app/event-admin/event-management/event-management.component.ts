import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'event-management',
  templateUrl: './event-management.component.html'
})
export class EventManagementComponent implements OnInit {
  eventId: string;

  constructor(
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
    });
  }
}
