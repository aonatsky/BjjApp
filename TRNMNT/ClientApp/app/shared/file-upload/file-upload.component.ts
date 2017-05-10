import { Observable } from 'rxjs/Rx';
import { Input, Component, ViewChild, EventEmitter, Output, ElementRef } from '@angular/core';
import { DataService } from "../../core/dal/contracts/data.service";
import { NotificationService } from "../../core/services/notification.service";


@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})


export class FileUpload {

    @ViewChild("fileInput") fileInput: ElementRef;

    constructor(private dataService: DataService, private notificationService: NotificationService) {

    }

    private upload() {
        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];
            this.dataService.uploadFighterList(fileToUpload).subscribe(result => this.processUploadResult(result));
        }
    }

    private processUploadResult(result: UploadResult) {
        if (result.code == 200) {
            this.notificationService.showSuccess("Processed", result.message)
        } else if (result.code == 201) {
            this.notificationService.showWarn("Porcessed with errors", result.message)
        } else if (result.code >= 500) {
            this.notificationService.showError("Fail", result.message)
        }


    }

    private addFile(): void {
        let fi = this.fileInput.nativeElement;
        fi.click();
    }



}

export interface UploadResult {
    code: number;
    message: string;
}
