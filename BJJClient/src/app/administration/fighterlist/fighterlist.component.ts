import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { DataTable, DataTableResource } from 'angular-2-data-table';



@Component({
    selector: 'fighterlist',
    templateUrl: './fighterlist.component.html',

})


export class FighterListComponent implements OnInit {
    
    fighters: Fighter[];
    rows = [
        { year: 1997, maker: 'Ford', model: 'E350', desc: 'ac, abs, moon', price: 3000.00 },
        { year: 1999, maker: 'Chevy', model: 'Venture "Extended Edition"', price: 4900.00 },
        { year: 1999, maker: 'Checy', model: 'Venture "Extended Edition, Very Large"', price: 5000.00 },
        { year: 1996, maker: 'Jeep', model: 'Grand Cherokee', desc: 'air, moon roof, loaded', price: 4799.00 }
    ];


    fighterResource : DataTableResource<Fighter>;
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

    

    populateTable(fighters:Fighter[]){
        this.fighterResource = new DataTableResource(fighters);
        this.fightersCount = fighters.length;
    }

    ngOnInit() {
        this.dataService.getFigters("Light").subscribe(data => this.populateTable(data))
        console.log("ON INIT")

    }
}

