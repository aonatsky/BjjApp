import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'




@Component({
    selector: 'brackets',
    templateUrl: './brackets.component.html',
    styleUrls: ['./brackets.component.css'],
    encapsulation: ViewEncapsulation.None

})


export class BracketsComponent implements OnInit {

    fightersCount: number = 8;
    rowsCount: number[];
    columnsCount: number[];
    fighters: Fighter[]

    tableArray: string[][];

    private dataService: DataService;


    constructor(_dataService: DataService) {
        this.dataService = _dataService;
    }

    //ng
    ngOnInit() {

        this.dataService.getFigters(null).subscribe(data => this.fighters = data);
    }

    formatFighterName(fighter:Fighter):string {
        return fighter.firstName + " " + fighter.lastName;
    }

    getFighterDiv(fighter:Fighter){
        return `<div class="input-group fighter">
      <div class="form-control">`+ this.formatFighterName(fighter) +`</div></div>`;
    }


    divFighter = `<div class="input-group fighter">
      <div class="form-control"></div></div>`;

    divleftToBottomSeparator =
    `<table class='bracket-separator left-to-bottom'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

    divleftToTopSeparator =
    `<table class='bracket-separator left-to-top'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

    divCenterToRightSeparator =
    `<table class='bracket-separator center-to-right'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

    divCenterSeparator =
    `<table class='bracket-separator center'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

    divBottomToRightSeparator =
    `<table class='bracket-separator bottom-to-right'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

    divTopToRightSeparator =
    `<table class='bracket-separator top-to-right'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;

    divLeftToCenterSeparator =
    `<table class='bracket-separator left-to-center'><tr><td></td><td></td></td></tr><tr><td></td><td></td></td></tr></table>`;


}

