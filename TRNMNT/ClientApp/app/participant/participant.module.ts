import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ParticipantComponent } from './participant.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { MyEventsComponent } from './my-events/my-events.component';
import { EventInfoPanelComponent } from './event-info-panel/event-info-panel.component';

@NgModule({
  imports: [CommonModule, SharedModule, CoreModule],
  declarations: [ParticipantComponent, MyEventsComponent, EventInfoPanelComponent]
})
export class ParticipantModule {}
