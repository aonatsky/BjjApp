import { Observable } from 'rxjs/Rx';
import { Component, EventEmitter, Output, Input } from '@angular/core';
import { NotificationService } from "../../core/services/notification.service";
import { ParticipantService } from '../../core/services/participant.service';
import { IUploadResult } from '../../core/model/result/upload-result.model';
import { UploadResultCode } from '../../core/model/enum/upload-result-code.enum';


@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})


export class FileUploadComponent {

    @Input() eventId: string;
    @Output() onUpload: EventEmitter<IUploadResult>;

    constructor(private notificationService: NotificationService, private participantService : ParticipantService) {
        this.onUpload = new EventEmitter<IUploadResult>();
    }

    private onFileUpload($event) {
        let fi = $event;
        if (fi.files && $event.files[0]) {
            let fileToUpload = fi.files[0];
            this.participantService.uploadParticipantsFromFile(fileToUpload, this.eventId).subscribe(result => this.processUploadResult(result), () => this.notificationService.showGenericError());
        }
    }

    private processUploadResult(result: IUploadResult) {
        if (result.code == UploadResultCode.Success) {
            this.notificationService.showSuccess("Processed", result.message);
        } else if (result.code == UploadResultCode.SuccessWithErrors) {
            this.notificationService.showWarn("Porcessed with errors", result.message);
        } else if (result.code >= UploadResultCode.Error) {
            this.notificationService.showError("Fail", result.message);
        }
        this.onUpload.emit(result);

    }

}


