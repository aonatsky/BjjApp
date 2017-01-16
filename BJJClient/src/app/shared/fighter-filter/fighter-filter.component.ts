import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core"
import { DropdownComponent } from '../dropdown/dropdown.component'
import { DataService } from '../../core/dal/contracts/data.service'
import { WeightClass } from '../../core/model/weight-class.model'
import { DefaultValues } from '../../core/consts/default-values'


@Component({
    selector: 'fighter-filter',
    templateUrl: 'fighter-filter.component.html',
    styleUrls: ['fighter-filter.component.css']
})

export class FighterFilter implements OnInit {

    dataService: DataService;
    weightClasses: WeightClass[];

    
    idProperty = "weightClassID";
    nameProperty = "name";

    @Output() onFilterChanged: EventEmitter<FighterFilterValue>
    @Output() currentFilterValue: FighterFilterValue;


    constructor(dataService: DataService) {
        this.dataService = dataService;
        this.onFilterChanged = new EventEmitter<FighterFilterValue>();
    }

    ngOnInit() {
       this.setupFilters();
    }

    //Events
    weightSelect(value) {
        this.currentFilterValue.weightClass = value;
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    //Private methods
    
    private setupFilters() {
        //weightClasses
        this.dataService.getWeightClasses().subscribe(data => this.setupWeightClassses(data))
        this.currentFilterValue = new FighterFilterValue(this.weightClasses[0])
    }

    private setupWeightClassses(weightClasses : WeightClass[]){
        this.weightClasses = [new WeightClass(DefaultValues.ANY,0)];
        this.weightClasses = this.weightClasses.concat(weightClasses);
    }

}


export class FighterFilterValue {
    weightClass: WeightClass;

    constructor(weightClass: WeightClass) {
        this.weightClass = weightClass;
    }

}
