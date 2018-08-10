﻿import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuItem } from 'primeng/primeng';
import { EventModel } from '../../core/model/event.models';
import { EventService } from '../../core/services/event.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.scss']
})
export class EventEditComponent implements OnInit {
  eventModel: EventModel;
  menuItems: MenuItem[] = [];
  currentStep: number = 0;
  categoryCount: number = 0;
  lastStep: number = 3;
  cacheRefreshToken: string;

  constructor(
    private eventService: EventService,
    private route: ActivatedRoute,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.initMenu();
    this.initData();
  }

  private initData() {
    this.route.params.subscribe(p => {
      let id = p['id'];
      if (id && id != '') {
        this.eventService.getEvent(id).subscribe(r => (this.eventModel = r));
      } else {
        alert('No data to display');
      }
    });
  }

  private initMenu() {
    this.menuItems = [
      {
        label: this.translateService.instant('EVENT_EDIT.GENERAL_INFORMATION')
      },
      {
        label: this.translateService.instant('EVENT_EDIT.PRICES')
      },
      {
        label: this.translateService.instant('EVENT_EDIT.CATEGORIES')
      },
      {
        label: this.translateService.instant('EVENT_EDIT.ADDITIONAL')
      }
    ];
  }

  private modelReload() {
    this.eventService.getEvent(this.eventModel.eventId).subscribe(r => {
      this.eventModel = r;
    });
  }

  private imageReload() {
    this.cacheRefreshToken = Date.now().toString();
    this.eventService.getEvent(this.eventModel.eventId).subscribe(r => {
      this.eventModel.imgPath = r.imgPath;
    });
  }
  
  private tncReload() {
    this.eventService.getEvent(this.eventModel.eventId).subscribe(r => {
      this.eventModel.tncFilePath = r.tncFilePath;
    });
  }


  private nextStep() {
    this.currentStep++;
  }

  private previousStep() {
    this.currentStep--;
  }

  private save() {
    this.eventService.updateEvent(this.eventModel).subscribe();
  }

  private delete() {
    this.eventService.deleteEvent(this.eventModel.eventId).subscribe();
  }

  private onImageUpload(event): void {
    this.eventService.uploadEventImage(event.files[0], this.eventModel.eventId).subscribe(r => this.imageReload());
  }

  private onTncUpload(event): void {
    this.eventService.uploadEventTncFile(event.files[0], this.eventModel.eventId).subscribe(r => {
      this.tncReload();
    });
  }

  private onPromoCodeUpload(event): void {
    this.eventService.uploadPromoCodeList(event.files[0], this.eventModel.eventId).subscribe(r => this.modelReload());
  }

  private downloadTnc(): void {
    this.eventService.downloadEventTncFile(this.eventModel.tncFilePath).subscribe();
  }

  getEventImageUrl(): string {
    return `${this.eventModel.imgPath}?${this.cacheRefreshToken}`;
  }
}
