import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '../../../../node_modules/@angular/router';
import { RouterService } from '../../core/services/router.service';

@Component({
  selector: 'app-event-dashboard',
  templateUrl: './event-dashboard.component.html',
  styleUrls: ['./event-dashboard.component.scss']
})
export class EventDashboardComponent implements OnInit {
  constructor(public route: ActivatedRoute, private routerService: RouterService, private translateService : TranslateService) {}
  eventId: AAGUID;

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
    });
  
    columns: CrudColumn[] = [
      {
        propertyName: 'name',
        displayName: this.translateService.instant('TEAM.NAME'),
        isEditable: false,
        isSortable: true
      },
      {
        propertyName: 'contactName',
        displayName: this.translateService.instant('TEAM.CONTACT_NAME'),
        isEditable: false,
        isSortable: true
      },
      {
        propertyName: 'contactPhone',
        displayName: this.translateService.instant('TEAM.CONTACT_PHONE'),
        isEditable: false,
        isSortable: true
      },
      {
        propertyName: 'contactEmail',
        displayName: this.translateService.instant('TEAM.CONTACT_EMAIL'),
        isEditable: false,
        isSortable: true
      },
      {
        propertyName: 'approvalStatus',
        displayName: this.translateService.instant('COMMON.APPROVAL.PAYMENT_APPROVAL_STATUS'),
        isEditable: false,
        isSortable: true,
        transform: value => this.translateService.instant(value)
      }
    ];
  
  }
}
