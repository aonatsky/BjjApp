import { NgModule } from '@angular/core';
import { CoreModule } from './../core/core.module';
import { RouterModule } from '@angular/router';
import { EventAdminPageComponent } from './event-admin.page.component';
import { EventOverviewComponent } from './event-overview/event-overview.component';
import { EventEditComponent } from './event-edit/event-edit.component';
import { eventAdminRoutes } from './event-admin.routing';
import { BracketGenerationComponent } from './event-management/brackets-generation/bracket-generation.component';
import { EventManagementComponent } from './event-management/event-management.component';
// tslint:disable-next-line:max-line-length
import { EventManagementParticipantsComponent } from './event-management-participants/event-management-participants.component';
import { SharedModule } from '../shared/shared.module';
import { PrticipantsListUploadComponent } from './participant-list-upload/participant-list-upload.component';
import { CategoryEditComponent } from './event-edit/category-edit/category-edit.component';
import { WebsocketInteractionComponent } from './websocket-interaction/websocket-interaction.component';
import { ConnectorComponent } from './event-management/brackets-generation/connector.component';
import { EventRunComponent } from './event-run/event-run.component';
import { RoundModule } from '../round/round.module';
import { EventRunWeightDivisionViewComponent } from './event-run-wd-view/event-run-wd-view.component';
import { EventRunCategoryViewComponent } from './event-run-category-view/event-run-category-view.component';
import { AbsoluteWeightDivisionComponent } from './absolute-weight-division/absolute-weight-division.component';
import { ResultsComponent } from './event-management/results/results.component';
import { BracketResultSetComponent } from './event-run/bracket-result-set/bracket-result-set.component';

@NgModule({
  imports: [CoreModule, SharedModule, RoundModule, RouterModule.forChild(eventAdminRoutes)],
  declarations: [
    EventAdminPageComponent,
    EventOverviewComponent,
    EventEditComponent,
    BracketGenerationComponent,
    EventManagementComponent,
    CategoryEditComponent,
    EventManagementParticipantsComponent,
    PrticipantsListUploadComponent,
    WebsocketInteractionComponent,
    EventRunComponent,
    EventRunWeightDivisionViewComponent,
    EventRunCategoryViewComponent,
    AbsoluteWeightDivisionComponent,
    ConnectorComponent,
    ResultsComponent,
    BracketResultSetComponent
  ],

  providers: [],

  exports: []
})
export class EventAdminModule {}
