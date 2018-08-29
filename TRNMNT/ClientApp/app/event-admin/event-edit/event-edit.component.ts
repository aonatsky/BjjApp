import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuItem, ConfirmationService } from 'primeng/primeng';
import { EventModel } from '../../core/model/event.models';
import { EventService } from '../../core/services/event.service';
import { TranslateService } from '@ngx-translate/core';
import { NgForm } from '@angular/forms';
import { RouterService } from '../../core/services/router.service';
import DateHelper from '../../core/helpers/date-helper';

@Component({
  selector: 'event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.scss'],
  providers: [ConfirmationService]
})
export class EventEditComponent implements OnInit {
  eventModel: EventModel;
  menuItems: MenuItem[] = [];
  currentStep: number = 0;
  categoryCount: number = 0;
  lastStep: number = 3;
  cacheRefreshToken: string;
  isUrlPrefixExists: boolean = false;
  @ViewChild('mainDataForm')
  mainDataForm: NgForm;
  @ViewChild('priceForm')
  priceForm: NgForm;

  constructor(
    private eventService: EventService,
    private route: ActivatedRoute,
    private translateService: TranslateService,
    private routerservice: RouterService,
    private confirmationService: ConfirmationService
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

  isDatesValid(): boolean {
    if (
      !this.eventModel.registrationStartTS ||
      !this.eventModel.earlyRegistrationEndTS ||
      !this.eventModel.eventDate ||
      !this.eventModel.registrationEndTS
    ) {
      return true;
    }
    return (
      this.eventModel.eventDate > this.eventModel.registrationEndTS &&
      this.eventModel.registrationEndTS > this.eventModel.earlyRegistrationEndTS &&
      this.eventModel.earlyRegistrationEndTS > this.eventModel.registrationStartTS &&
      this.eventModel.eventDate > DateHelper.getCurrentDate()
    );
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

  isEventValid(): boolean {
    return (
      (this.currentStep == 0 &&
        !!this.mainDataForm &&
        this.mainDataForm.valid &&
        !!this.eventModel.tncFilePath &&
        this.isDatesValid()) ||
      (!!this.priceForm && this.priceForm.valid && this.currentStep == 1)||(this.currentStep == 2)||(this.currentStep == 3)
    );
  }

  nextStep() {
    this.currentStep++;
  }

  previousStep() {
    this.currentStep--;
  }

  save() {
    this.eventService.updateEvent(this.eventModel).subscribe();
  }

  deleteEvent() {
    this.showConfirm(this.delete);
  }

  private delete() {
    this.eventService.deleteEvent(this.eventModel.eventId).subscribe(() => this.routerservice.goToEventAdmin());
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

  showConfirm(method: Function) {
    this.confirmationService.confirm({
      header: this.translateService.instant('COMMON.CONFIRMATION'),
      icon: 'fa fa fa-trash',
      message: this.translateService.instant('EVENT_EDIT.DO_YOU_WANT_TO_DELETE_EVENT'),
      acceptLabel: this.translateService.instant('COMMON.YES'),
      rejectLabel: this.translateService.instant('COMMON.NO'),
      accept: () => method()
    });
  }

  checkIfPrefixExist() {
    this.eventService
      .isPrefixExists(this.eventModel.eventId, this.eventModel.urlPrefix)
      .subscribe(r => (this.isUrlPrefixExists = r));
  }
}
