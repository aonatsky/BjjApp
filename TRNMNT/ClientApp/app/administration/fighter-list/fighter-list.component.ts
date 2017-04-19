import {CrudColumn} from '../../shared/crud/crud.component';
import { Component, OnInit, ViewChild, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { FighterFilter } from '../../shared/fighter-filter/fighter-filter.component'



@Component({
    selector: 'fighterlist',
    templateUrl: './fighter-list.component.html',
    styleUrls: ['./fighter-list.component.css']
})


export class FighterListComponent implements OnInit, AfterViewInit {

    fighters: Fighter[];
    fighterColumns: CrudColumn[] = [
        { displayName: "First Name", propertyName: "firstName", isEditable: false },
        { displayName: "Last Name", propertyName: "lastName", isEditable: false },
        { displayName: "DOB", propertyName: "dateOfBirth", isEditable: false },
        { displayName: "Team", propertyName: "team", isEditable: false },
        { displayName: "Category", propertyName: "team", isEditable: false }
    ];
    
    @ViewChild(FighterFilter) fighterFilter: FighterFilter;
    constructor(private dataService: DataService) {
    }

    

    //ng
    ngOnInit() {
    }

    ngAfterViewInit() {
        this.refreshTable();
    }


    //events
    onFilterChanged() {
        this.refreshTable();
    }

    private refreshTable() {
        this.dataService.getFigters(this.fighterFilter.currentFilterValue).subscribe(data => this.fighters = data)
    }
}

