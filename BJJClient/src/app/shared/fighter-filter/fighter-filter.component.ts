import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core"
import { DropdownComponent } from '../dropdown/dropdown.component'
import { DataService } from '../../core/dal/contracts/data.service'
import { WeightClass } from '../../core/model/weight-class.model'


@Component({
    selector: 'fighter-filter',
    templateUrl: 'fighter-filter.component.html',
    styleUrls: ['fighter-filter.component.css']
})

export class FighterFilter implements OnInit {

    dataService: DataService;
    weightClasses: WeightClass[];

    currentFilterValue: FighterFilterValue;

    idProperty = "weightClassID";
    nameProperty = "name";

    @Output() onFilterChanged: EventEmitter<FighterFilterValue>

    constructor(dataService: DataService) {
        this.dataService = dataService;
        this.onFilterChanged = new EventEmitter<FighterFilterValue>();
    }

    ngOnInit() {
        this.dataService.getWeightClasses().subscribe(data => this.weightClasses = data)
        this.currentFilterValue = new FighterFilterValue(this.weightClasses[0])
    }

    weightSelect(value) {
        this.currentFilterValue.weightClass = value;
        this.onFilterChanged.emit(this.currentFilterValue);
    }
}


export class FighterFilterValue {
    weightClass: WeightClass;

    constructor(weightClass: WeightClass) {
        this.weightClass = weightClass;
    }

}
