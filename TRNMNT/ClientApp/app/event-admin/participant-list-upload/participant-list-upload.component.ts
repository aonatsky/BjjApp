import { Observable } from 'rxjs';
import { Component, EventEmitter, Output, Input } from '@angular/core';
import { NotificationService } from '../../core/services/notification.service';
import { ParticipantService } from '../../core/services/participant.service';
import { IUploadResult } from '../../core/model/result/upload-result.model';
import { UploadResultCode } from '../../core/model/enum/upload-result-code.enum';
import { Message } from 'primeng/components/common/message';
import { MessageLevel } from '../../core/consts/message-level.const';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'participant-list-upload',
  templateUrl: './participant-list-upload.component.html'
})
export class ParticipantsListUploadComponent {
  @Input() eventId: string;
  @Input() hideMessageDelay: number = 10000;

  @Output() onUpload: EventEmitter<IUploadResult>;

  uploadResultMessages: Message[] = [];

  constructor(
    private notificationService: NotificationService,
    private participantService: ParticipantService,
    private translateService: TranslateService
  ) {
    this.onUpload = new EventEmitter<IUploadResult>();
  }

  onFileUpload($event) {
    let fi = $event;
    if (fi.files && $event.files[0]) {
      let fileToUpload = fi.files[0];
      this.participantService.uploadParticipantsFromFile(fileToUpload, this.eventId).subscribe(
        result => this.processUploadResult(result),
        () => {
          this.uploadResultMessages = [];
          this.uploadResultMessages.push(this.notificationService.genericErrorMessage);
        }
      );
    }
  }

  private processUploadResult(result: IUploadResult) {
    if (result.code == UploadResultCode.Success) {
      this.showMessage(result, MessageLevel.Success, this.translateService.instant('UPLOAD_RESULT.PROCESSED'));
    } else if (result.code == UploadResultCode.SuccessWithErrors) {
      this.showMessage(result, MessageLevel.Warn, this.translateService.instant('UPLOAD_RESULT.PROCESSED_WITH_ERRORS'));
    } else if (result.code >= UploadResultCode.Error) {
      this.showMessage(result, MessageLevel.Error, this.translateService.instant('UPLOAD_RESULT.FAIL'));
    }
    this.onUpload.emit(result);
  }

  showMessage(result: IUploadResult, level: string, title: string) {
    this.uploadResultMessages = [];
    result.messages.map(m => this.uploadResultMessages.push(this.notificationService.getMessage(level, title, m)));
  }
}
