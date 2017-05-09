import { Observable } from 'rxjs/Rx';
import { Input, Component, ViewChild, EventEmitter, Output, ElementRef } from '@angular/core';
import { DataService } from "../../core/dal/contracts/data.service";
//import { NotificationService } from "../../core/services/notification.service";

import { Message } from 'primeng/primeng';


@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})


export class FileUpload {

    @ViewChild("fileInput") fileInput: ElementRef;
    uploadMessages: Message[] = [];

    constructor(private dataService: DataService) {

    }

    private upload() {
        this.uploadMessages = [];
        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];
            this.dataService.uploadFighterList(fileToUpload).subscribe(result => this.processUploadResult(result));
        }
        fi.files = [];
    }

    private processUploadResult(result: UploadResult) {
        let severity = "success";
        let summary = "Processed";
        if (result.code == 201) {
            summary = "Porcessed with errors"
            severity = "warn";
        } else if (result.code >= 500) {
            summary = "Fail"
            severity = "error";
        }
        let message = { severity: severity, summary: summary, detail: result.message }
        //this.notificationService.addNotification(message)

        //this.uploadMessages.push({ severity: severity, summary: summary, detail: result.message })

    }

    addFile(): void {
        let fi = this.fileInput.nativeElement;
        fi.click();
    }



}

export interface UploadResult {
    code: number;
    message: string;
}
