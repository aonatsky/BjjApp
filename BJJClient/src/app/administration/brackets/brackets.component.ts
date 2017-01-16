import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fight } from '../../core/model/fight.model'




@Component({
    selector: 'brackets',
    templateUrl: './brackets.component.html',
    styleUrls: ['./brackets.component.css']

})


export class BracketsComponent {

    fights: Fight[];
    private dataService: DataService;


    constructor(_dataService: DataService) {
        this.dataService = _dataService;
    }


    
}

