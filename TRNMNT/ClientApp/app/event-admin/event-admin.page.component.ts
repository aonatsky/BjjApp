import { Component, OnInit } from '@angular/core';
import { LoggerService } from '../core/services/logger.service';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'event-admin',
  templateUrl: './event-admin.page.component.html',
  styleUrls: ['./event-admin.page.component.scss']
})
export class EventAdminPageComponent implements OnInit {
  userData: string;

  constructor(private loggerService: LoggerService, private authService: AuthService) {}

  ngOnInit() {
    this.userData = JSON.stringify(this.authService.getUser());
  }
}
