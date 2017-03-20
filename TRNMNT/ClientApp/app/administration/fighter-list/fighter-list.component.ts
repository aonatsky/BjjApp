import { Component, OnInit, ViewChild, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { FighterFilter } from '../../shared/fighter-filter/fighter-filter.component'
import { DefaultValues } from '../../core/consts/default-values'



@Component({
    selector: 'fighterlist',
    templateUrl: './fighter-list.component.html',
    styleUrls: ['./fighter-list.component.css']
})


export class FighterListComponent implements OnInit, AfterViewInit {

    fighters: Fighter[];
    @ViewChild(FighterFilter) fighterFilter: FighterFilter;
    constructor(private dataService: DataService) {
    }

    populateTable(fighters: Fighter[]) {
        this.fighters = fighters;
    }

    //ng
    ngOnInit() {
    }

    ngAfterViewInit() {
        this.initTable();
    }


    //events
    onFilterChanged() {
        this.initTable();
    }

    private initTable() {
        this.dataService.getFigters(this.fighterFilter.currentFilterValue).subscribe(data => this.populateTable(data))
    }
}

