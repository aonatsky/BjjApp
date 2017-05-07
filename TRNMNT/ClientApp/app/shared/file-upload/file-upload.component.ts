import { Input, Component, ViewChild, EventEmitter, Output } from '@angular/core';
import { DataService } from "../../core/dal/contracts/data.service";


@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html'

})


export class FileUpload {
    @Input() actionUrl: string;
    @ViewChild("fileInput") fileInput;
    @Output() onUpload: EventEmitter<any> = new EventEmitter<any>();

    /**
     *
     */
    constructor() {

    }

    upload() {
        alert("hey");
    }

    addFile(): void {
        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];
            this.onUpload.emit(fileToUpload);
        }
    }
}