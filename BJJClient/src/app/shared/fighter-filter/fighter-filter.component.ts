import {AgeDivision} from '../../core/model/age-division.model';
import {BeltDivision} from '../../core/model/belt-division.model';
import {WeightDivision} from '../../core/model/weight-division.model';
import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core"
import { DropdownComponent } from '../dropdown/dropdown.component'
import { DataService } from '../../core/dal/contracts/data.service'
import { DefaultValues } from '../../core/consts/default-values'


@Component({
    selector: 'fighter-filter',
    templateUrl: 'fighter-filter.component.html',
    styleUrls: ['fighter-filter.component.css']
})

export class FighterFilter implements OnInit {

    weightDivisions: WeightDivision[];
    beltDivisions: BeltDivision[];
    ageDivisions: AgeDivision[];
    
    WeightDivisionIdPropertyName = "weightDivisionID";
    AgetDivisionIdPropertyName = "ageDivisionID";
    BeltDivisionIdPropertyName = "beltDivisionID";
    nameProperty = "name";

    @Output() onFilterChanged: EventEmitter<FighterFilterValue>
    @Output() currentFilterValue: FighterFilterValue;


    constructor(private dataService: DataService) {
        this.onFilterChanged = new EventEmitter<FighterFilterValue>();
    }

    ngOnInit() {
       this.setupFilters();
    }

    //Events
    weightSelect(value) {
        this.currentFilterValue.weightDivision = value;
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    //Private methods
    
    private setupFilters() {
        //weightClasses
        this.dataService.getWeightDivisions().subscribe(data => this.setupWeightDivisions(data))
        this.dataService.getAgeDivisions().subscribe(data => this.setupAgeDivisions(data))
        this.dataService.getBeltDivisions().subscribe(data => this.setupBeltDivisions(data))
        
        this.currentFilterValue = new FighterFilterValue(this.weightDivisions[0])
    }

    private setupWeightDivisions(weightDivisions : WeightDivision[]){
        this.weightDivisions = [new WeightDivision(DefaultValues.DROPDOWN_ID_ANY,DefaultValues.DROPDOWN_VALUE_ANY,0)];
        this.weightDivisions = this.weightDivisions.concat(weightDivisions);
    }
    private setupBeltDivisions(beltDivisions : BeltDivision[]){
        this.beltDivisions = [new BeltDivision(DefaultValues.DROPDOWN_ID_ANY,DefaultValues.DROPDOWN_VALUE_ANY)];
        this.beltDivisions = this.beltDivisions.concat(beltDivisions);
    }
    private setupAgeDivisions(ageDivisions : AgeDivision[]){
        this.ageDivisions = [new AgeDivision(DefaultValues.DROPDOWN_ID_ANY,DefaultValues.DROPDOWN_VALUE_ANY,0)];
        this.ageDivisions = this.ageDivisions.concat(ageDivisions);
    }

}


export class FighterFilterValue {
    constructor(public weightDivision: WeightDivision) {
    }
}
