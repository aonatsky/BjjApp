import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fight } from '../../core/model/fight.model'
import { Fighter } from '../../core/model/fighter.model'




@Component({
    selector: 'brackets',
    templateUrl: './brackets.component.html',
    styleUrls: ['./brackets.component.css']

})


export class BracketsComponent implements OnInit {

    fightersCount: number = 8;
    rowsCount: number[];
    columnsCount: number[];
    fighters: Fighter[]


    fights: Fight[];
    private dataService: DataService;


    constructor(_dataService: DataService) {
        this.dataService = _dataService;
    }

    //ng
    ngOnInit() {

        this.dataService.getFigters(null).subscribe(data => this.fighters = data);
        this.rowsCount = this.getNumbersArray(this.getRowsCount());
        this.columnsCount = this.getNumbersArray(this.getColumnsCount());

    }


    getRowsCount(): number {
        return this.fightersCount / 2 * 2;

    }
    getColumnsCount(): number {
        let rounds = (Math.log2(this.fightersCount / 2) * 4) + 3;
        return this.fightersCount / 2 * 2;

    }

    getNumbersArray(count: number): number[] {
        return Array(count).fill(0).map((x, i) => i); // [0,1,2,3,4]
    }




}

