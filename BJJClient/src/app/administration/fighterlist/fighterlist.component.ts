import { Component, OnInit } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { DataFakeService } from '../../core/dal/fake/data-fake.service'
import { ApiProviders } from '../../core/dal/api.providers'
import { Fighter } from '../../core/model/fighter.model'

@Component({
    selector: 'fighterlist',
    templateUrl: './fighterlist.component.html',
    providers:[ApiProviders]

})


export class FighterListComponent implements OnInit {
    public fighters: Fighter[];

    private dataService: DataService;

    constructor(_dataService: DataService) {
        this.dataService = _dataService
        //console.log(_dataService.getFigters().on)
    }

    ngOnInit() {
        this.dataService.getFigters("w").subscribe(d => console.log(d.toString()))
    }
}

class DropdownValue {
    value: string;
    label: string;
}


class weightDivision {
    id: string;
    name: string;

    constructor(id, name) {
        this.id = id;
        this.name = name;
    }
}

