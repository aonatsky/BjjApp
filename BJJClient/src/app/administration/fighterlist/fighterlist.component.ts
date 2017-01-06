import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { ApiProviders } from '../../core/dal/api.providers'
import { Fighter } from '../../core/model/fighter.model'
import { DataTable, DataTableResource } from 'angular-2-data-table';



@Component({
    selector: 'fighterlist',
    templateUrl: './fighterlist.component.html',

})


export class FighterListComponent implements OnInit {
    public fighters: Fighter[];
    // public columns = [
    //     {prop: 'firstName'},
    //     {prop: 'lastName'},
    // ]

rows = [
    { year: 1997, maker: 'Ford', model: 'E350', desc: 'ac, abs, moon', price: 3000.00 },
    { year: 1999, maker: 'Chevy', model: 'Venture "Extended Edition"', price: 4900.00 },
    { year: 1999, maker: 'Checy', model: 'Venture "Extended Edition, Very Large"', price: 5000.00 },
    { year: 1996, maker: 'Jeep', model: 'Grand Cherokee', desc: 'air, moon roof, loaded', price: 4799.00 }
];
  
  columns = [
    { prop: 'name' },
    { name: 'Gender' },
    { name: 'Company' }
  ];


    private dataService: DataService;

    constructor(_dataService: DataService) {
        this.dataService = _dataService
        //console.log(_dataService.getFigters().on)
    }

    ngOnInit() {
        this.dataService.getFigters("w").subscribe(data => this.fighters = data)

    }
}

