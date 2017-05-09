import { Observable } from 'rxjs/Rx';
import { Input, Component, ViewChild, EventEmitter, Output, ElementRef } from '@angular/core';
import { DataService } from "../../core/dal/contracts/data.service";


@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})


export class FileUpload {

    @ViewChild("fileInput") fileInput: ElementRef;
    @ViewChild("uploadResult") uploadResultSpan: ElementRef;
    isError: boolean = false;

    constructor(private dataService: DataService) {

    }

    private upload() {
        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];
            this.dataService.uploadFighterList(fileToUpload).subscribe(result => this.processUploadResult(result));
        }
    }

    private processUploadResult(result: UploadResult) {
        let span = this.uploadResultSpan.nativeElement;
        this.isError = result.code >= 500;
        span.innerHTML = result.message;
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
