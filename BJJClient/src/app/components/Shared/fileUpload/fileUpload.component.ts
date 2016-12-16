import { Input, Component } from '@angular/core';

@Component({
    selector : 'file-upload'
    
})


export class FileUpload {
    @Input() actionUrl : string;

}