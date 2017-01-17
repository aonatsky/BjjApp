import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { DataTable, DataTableResource } from 'angular-2-data-table';
import { FighterFilter, FighterFilterValue } from '../../shared/fighter-filter/fighter-filter.component'
import { DefaultValues } from '../../core/consts/default-values'



@Component({
    selector: 'fighterlist',
    templateUrl: './fighterlist.component.html',
    styleUrls: ['./fighterlist.component.css']

})


export class FighterListComponent implements OnInit, AfterViewInit {

    fighters: Fighter[];


    fighterResource: DataTableResource<Fighter>;
    fightersCount = 0;

    @ViewChild(DataTable) fightersTable: DataTable;
    @ViewChild(FighterFilter) fighterFilter: FighterFilter;

    private dataService: DataService;

    constructor(_dataService: DataService) {
        this.dataService = _dataService;
    }

    populateTable(fighters: Fighter[]) {
        this.fighters = fighters;
        this.fighterResource = new DataTableResource(fighters);
        this.fightersCount = fighters.length;
    }

    //ng
    ngOnInit() {

    }
    
    ngAfterViewInit() {
        
        this.dataService.getFigters(this.getWeightClassFromFilter()).subscribe(data => this.populateTable(data))
    }

    
    //events
    onFilterChanged() {
        this.dataService.getFigters(this.getWeightClassFromFilter()).subscribe(data => this.populateTable(data))
    }

    //private methods
    private getWeightClassFromFilter():string{
         let weightClassFromFilter = this.fighterFilter.currentFilterValue.weightClass.name == DefaultValues.ANY
            ? null
            : this.fighterFilter.currentFilterValue.weightClass.name;
        return weightClassFromFilter;
    }

 

}

