import { Component, Input, Output, OnInit } from "@angular/core"
import { DropdownComponent } from '../dropdown/dropdown.component'
import { DataService } from '../../core/dal/contracts/data.service'
import { WeightClass } from '../../core/model/weight-class.model'


@Component({
    selector: 'fighter-filter',
    template: `<dropdown [dropdownValues]="weightClases"></dropdown>`
})

export class FighterFilter implements OnInit {

    dataService: DataService;
    weightClases: WeightClass[];

    constructor(dataService: DataService) {
        this.dataService = dataService;
    }

    ngOnInit() {
        this.dataService.getWeightClasses().subscribe(data => this.weightClases = data)
    }
}

