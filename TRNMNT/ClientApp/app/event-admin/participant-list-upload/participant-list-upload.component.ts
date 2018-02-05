import { Observable } from 'rxjs/Rx';
import { Component, EventEmitter, Output, Input } from '@angular/core';
import { NotificationService } from "../../core/services/notification.service";
import { ParticipantService } from '../../core/services/participant.service';
import { IUploadResult } from '../../core/model/result/upload-result.model';
import { UploadResultCode } from '../../core/model/enum/upload-result-code.enum';
import { Message } from 'primeng/components/common/message';
import { MessageLevel } from '../../core/consts/message-level.const';



@Component({
    selector: 'participant-list-upload',
    templateUrl: './participant-list-upload.component.html',
})


export class PrticipantsListUploadComponent {

    @Input() eventId: string;
    @Input() hideMessageDelay: number = 10000;

    @Output() onUpload: EventEmitter<IUploadResult>;

    private uploadResultMessages: Message[] = [];

    constructor(private notificationService: NotificationService, private participantService: ParticipantService) {
        this.onUpload = new EventEmitter<IUploadResult>();
    }

    private onFileUpload($event) {
        let fi = $event;
        if (fi.files && $event.files[0]) {
            let fileToUpload = fi.files[0];
            this.participantService.uploadParticipantsFromFile(fileToUpload, this.eventId).subscribe(result => this.processUploadResult(result),
                () => {
                    this.uploadResultMessages = [];
                    this.uploadResultMessages.push(this.notificationService.genericErrorMessage);
                });
        }
    }

    private processUploadResult(result: IUploadResult) {
        if (result.code == UploadResultCode.Success) {
            this.showMessage(result, MessageLevel.Success, "Processed");
        } else if (result.code == UploadResultCode.SuccessWithErrors) {
            this.showMessage(result, MessageLevel.Warn, "Porcessed with errors");
        } else if (result.code >= UploadResultCode.Error) {
            this.showMessage(result, MessageLevel.Error, "Fail");
        }
        //Observable.interval(this.hideMessageDelay).take(1).subscribe(() => this.uploadResultMessages = []);
        this.onUpload.emit(result);
    }

    showMessage(result: IUploadResult, level: string, title: string) {
        this.uploadResultMessages = [];
        result.messages.map(m => this.uploadResultMessages.push(this.notificationService.getMessage(level, title, m)));
    }

}


