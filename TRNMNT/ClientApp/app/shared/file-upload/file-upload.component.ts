import { Input, Component, ViewChild } from '@angular/core';
import { DataService } from "../../core/dal/contracts/data.service";


@Component({
    selector: 'file-upload',
    template: `<input #fileInput type="file"/><button (click)="addFile()">Add</button>`

})


export class FileUpload {
    @Input() actionUrl: string;
    @ViewChild("fileInput") fileInput;

    /**
     *
     */
    constructor(private dataService: DataService) {

    }

    addFile(): void {
        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];
            this.dataService
                .uploadFighterList(fileToUpload)
                .subscribe(res => {
                    console.log(res);
                });
        }
    }
}