import {DataService} from '../../core/dal/contracts/data.service';
import {FileUpload} from '../../shared/file-upload/file-upload.component';
import { Component } from '@angular/core';
import { BracketsComponent } from './../Brackets/brackets.component'
import { TournamentSettingsComponent } from './../tournament-settings/tournament-settings.component';


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

    uploadFile(file){
        this.dataService.uploadFighterList(file).subscribe();
    }
}
