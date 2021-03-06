import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '../../../../node_modules/@angular/router';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '@ngx-translate/core';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { EventDashboardModel } from '../../core/model/event.models';
import { EventService } from '../../core/services/event.service';
import { SelectItem } from 'primeng/primeng';

@Component({
  selector: 'event-dashboard',
  templateUrl: './event-dashboard.component.html',
  styleUrls: ['./event-dashboard.component.scss']
})
export class EventDashboardComponent implements OnInit {
  eventId: AAGUID;
  model: EventDashboardModel;
  sortField: string = 'categoryName';
  totalCount: number;
  columns: CrudColumn[] = [
    {
      propertyName: 'categoryName',
      displayName: this.translateService.instant('COMMON.CATEGORY'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'weightDivisionName',
      displayName: this.translateService.instant('COMMON.WEIGHT_DIVISION'),
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

  booleanSelectItems: SelectItem[] = [
    { label: this.translateService.instant('COMMON.YES'), value: true },
    { label: this.translateService.instant('COMMON.NO'), value: false }
  ];
  constructor(
    public route: ActivatedRoute,
    public routerService: RouterService,
    private translateService: TranslateService,
    private eventService: EventService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
      this.eventService.getEventDashboardData(this.eventId).subscribe(r => {
        this.model = r;
        this.totalCount = this.model.participantGroups.map(pg => pg.participantsCount).reduce((a, b) => a + b);
      });
    });
  }

  setCorrectionsEnabled($event) {
    if (!($event.value && !this.model.bracketsPublished && this.model.bracketsCreated)) {
      this.eventService
        .setCorrectionsEnabled(this.model.eventId, $event.checked)
        .subscribe(() => (this.model.correctionsEnabled = $event.checked));
    }
  }

  setParticipantListsPublished($event) {
    this.eventService.setParticipantListsPublish(this.model.eventId, $event.checked).subscribe();
  }

  setBracketsPublished($event) {
    this.eventService.setBracketsPublish(this.model.eventId, $event.checked).subscribe();
  }
}
