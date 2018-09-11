import { NgModule } from '@angular/core';
import { CoreModule } from '../core/core.module';
import { EventAdminPageComponent } from './event-admin.page.component';
import { EventListComponent } from './event-list/event-list.component';
import { EventEditComponent } from './event-edit/event-edit.component';
import { EventAdminRoutingModule } from './event-admin.routing.module';
import { BracketGenerationComponent } from './event-management/brackets-generation/bracket-generation.component';
import { EventManagementComponent } from './event-management/event-management.component';
// tslint:disable-next-line:max-line-length
import { EventManagementParticipantsComponent } from './event-management-participants/event-management-participants.component';
import { SharedModule } from '../shared/shared.module';
import { ParticipantsListUploadComponent } from './participant-list-upload/participant-list-upload.component';
import { CategoryEditComponent } from './event-edit/category-edit/category-edit.component';
import { ConnectorComponent } from './event-management/brackets-generation/connector.component';
import { EventRunComponent } from './event-run/event-run.component';
import { RoundModule } from '../round/round.module';
import { EventRunWeightDivisionViewComponent } from './event-run-wd-view/event-run-wd-view.component';
import { EventRunCategoryViewComponent } from './event-run-category-view/event-run-category-view.component';
import { AbsoluteWeightDivisionComponent } from './absolute-weight-division/absolute-weight-division.component';
import { ResultsComponent } from './event-management/results/results.component';
import { BracketResultSetComponent } from './event-run/bracket-result-set/bracket-result-set.component';
import { TeamListComponent } from './team-list/team-list.component';
import { TranslateModule } from '@ngx-translate/core';
import { FederationEditComponent } from './federation-edit/federation-edit.component';
import { EventDashboardComponent } from './event-dashboard/event-dashboard.component';

@NgModule({
  imports: [CoreModule, SharedModule, RoundModule, EventAdminRoutingModule, TranslateModule],
  declarations: [
    EventAdminPageComponent,
    EventDashboardComponent,
    EventListComponent,
    EventEditComponent,
    BracketGenerationComponent,
    EventManagementComponent,
    CategoryEditComponent,
    EventManagementParticipantsComponent,
    ParticipantsListUploadComponent,
    EventRunComponent,
    EventRunWeightDivisionViewComponent,
    EventRunCategoryViewComponent,
    AbsoluteWeightDivisionComponent,
    ConnectorComponent,
    ResultsComponent,
    BracketResultSetComponent,
    TeamListComponent,
    FederationEditComponent
  ],

  providers: [],

  exports: []
})
export class EventAdminModule {}
