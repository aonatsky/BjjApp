import {DataService} from '../../core/dal/contracts/data.service';
import {FileUpload} from '../../shared/file-upload/file-upload.component';
import { Component } from '@angular/core';


@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    /**
     *
     */
    constructor(private dataService: DataService) {
        
    }
    
}
