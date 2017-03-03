
import { AgeDivision } from '../../core/model/age-division.model';
import { BeltDivision } from '../../core/model/belt-division.model';
import { WeightDivision } from '../../core/model/weight-division.model';
import { FighterFilterModel } from '../../core/model/fighter-filter.model';
import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core"
import { DropdownComponent, DropDownListOption } from '../dropdown/dropdown.component'
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

    weightDivisionDDLOptions: DropDownListOption[] = [];
    ageDivisionDDLOptions: DropDownListOption[] = [];
    beltDivisionDDLOptions: DropDownListOption[] = [];

    @Output() onFilterChanged: EventEmitter<FighterFilterModel>
    @Output() currentFilterValue: FighterFilterModel = new FighterFilterModel([], [], []);


    constructor(private dataService: DataService) {
        this.onFilterChanged = new EventEmitter<FighterFilterModel>();
    }

    ngOnInit() {
        this.getData();
        this.currentFilterValue = new FighterFilterModel(this.weightDivisions, this.beltDivisions, this.ageDivisions);
        this.setupFilters();
    }

    //Events
    weightSelect(value) {
        if (value.name == DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.weightDivisions = this.weightDivisions;
        }
        else {
            this.currentFilterValue.weightDivisions = this.weightDivisions.filter(wd => wd.weightDivisionId == value.id);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    ageSelect(value) {
        if (value.name == DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.ageDivisions = this.ageDivisions;
        }
        else {
            this.currentFilterValue.ageDivisions = this.ageDivisions.filter(ad => ad.ageDivisionId == value.id);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    beltSelect(value) {
            if (value.name == DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.beltDivisions = this.beltDivisions;
        }
        else {
            this.currentFilterValue.beltDivisions = this.beltDivisions.filter(bd => bd.beltDivisionId == value.id);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    //Private methods

    private setupFilters() {
        let defaultDDLOption = new DropDownListOption(DefaultValues.DROPDOWN_ID_ANY, DefaultValues.DROPDOWN_NAME_ANY);
        this.weightDivisionDDLOptions.push(defaultDDLOption)
        this.weightDivisions.map(wd => this.weightDivisionDDLOptions.push(new DropDownListOption(wd.weightDivisionId, wd.name)))
        this.ageDivisionDDLOptions.push(defaultDDLOption)
        this.ageDivisions.map(ad => this.ageDivisionDDLOptions.push(new DropDownListOption(ad.ageDivisionId, ad.name)))
        this.beltDivisionDDLOptions.push(defaultDDLOption)
        this.beltDivisions.map(wd => this.beltDivisionDDLOptions.push(new DropDownListOption(wd.beltDivisionId, wd.name)))

    }

    private getData() {
        this.dataService.getWeightDivisions().subscribe(data => this.weightDivisions = data)
        this.dataService.getAgeDivisions().subscribe(data => this.ageDivisions = data)
        this.dataService.getBeltDivisions().subscribe(data => this.beltDivisions = data)
    }
}



