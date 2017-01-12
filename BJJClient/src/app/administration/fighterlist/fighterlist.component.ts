import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { DataTable, DataTableResource } from 'angular-2-data-table';
import { FighterFilter, FighterFilterValue } from '../../shared/fighter-filter/fighter-filter.component'



@Component({
    selector: 'fighterlist',
    templateUrl: './fighterlist.component.html',

})


export class FighterListComponent implements OnInit {

    fighters: Fighter[];


    fighterResource: DataTableResource<Fighter>;
    fightersCount = 0;

    @ViewChild(DataTable) fightersTable: DataTable;

    private dataService: DataService;

    constructor(_dataService: DataService) {
        this.dataService = _dataService;
    }

    reloadCars(params) {
        //this.fighterResource.query(params).then(data => this.fighters = data);
    }

    // custom features:

    carClicked(car) {
        alert(car.model);
    }



    populateTable(fighters: Fighter[]) {
        this.fighters = fighters;
        this.fighterResource = new DataTableResource(fighters);
        this.fightersCount = fighters.length;
    }

    ngOnInit() {
        this.dataService.getFigters("Light").subscribe(data => this.populateTable(data))
    }

    onFilterChanged(value:FighterFilterValue){
        this.dataService.getFigters(value.weightClass.name).subscribe(data => this.populateTable(data))
    }
    
}

