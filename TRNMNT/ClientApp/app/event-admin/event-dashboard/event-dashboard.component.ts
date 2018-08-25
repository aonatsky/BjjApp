import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '../../../../node_modules/@angular/router';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '@ngx-translate/core';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { EventDashboardModel } from '../../core/model/event.models';
import { EventService } from '../../core/services/event.service';

@Component({
  selector: 'event-dashboard',
  templateUrl: './event-dashboard.component.html',
  styleUrls: ['./event-dashboard.component.scss']
})
export class EventDashboardComponent implements OnInit {
  constructor(
    public route: ActivatedRoute,
    private routerService: RouterService,
    private translateService: TranslateService,
    private eventService: EventService
  ) {}
  eventId: AAGUID;
  model: EventDashboardModel;
  sortField: string = 'categoryName';
  totalCount: number;

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
      this.eventService.getEventDashboardData(this.eventId).subscribe(r => {
        this.model = r;
        this.totalCount = this.model.participantGroups
        .map(pg => pg.participantsCount).reduce((a, b) => a + b);
      });
    });
  }
  columns: CrudColumn[] = [
    {
      propertyName: 'categoryName',
      displayName: this.translateService.instant('COMMON.CATEGORY'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'weightDivisionName',
      displayName: this.translateService.instant('COMMON.WEIGHTDIVISION'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'participantsCount',
      displayName: this.translateService.instant('EVENT.PARTICIPANT_COUNT'),
      isEditable: false,
      isSortable: true
    }
  ];
}
