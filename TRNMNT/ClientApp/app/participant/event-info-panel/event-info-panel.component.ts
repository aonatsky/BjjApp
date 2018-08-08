import { Component, OnInit, Input } from '@angular/core';
import { EventPreviewModel } from '../../core/model/event.models';

@Component({
  selector: 'app-event-info-panel',
  templateUrl: './event-info-panel.component.html',
  styleUrls: ['./event-info-panel.component.scss']
})
export class EventInfoPanelComponent implements OnInit {

@Input() eventModel : EventPreviewModel;
  constructor() { }

  ngOnInit() {
  }

  
}
