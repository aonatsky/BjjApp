import {CrudColumn} from '../../shared/crud/crud.component';
import { Component, OnInit, ViewChild, AfterViewInit, ViewEncapsulation, AfterContentInit } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { FighterFilter } from '../../shared/fighter-filter/fighter-filter.component'
import { FileUpload } from '../../shared/file-upload/file-upload.component'



@Component({
    selector: 'fighterlist',
    templateUrl: './fighter-list.component.html',
    styleUrls: ['./fighter-list.component.css']
})


export class FighterListComponent implements OnInit, AfterContentInit {

    fighters: Fighter[];
    fighterColumns: CrudColumn[] = [
        { displayName: "First Name", propertyName: "firstName", isEditable: false },
        { displayName: "Last Name", propertyName: "lastName", isEditable: false },
        { displayName: "DOB", propertyName: "dateOfBirth", isEditable: false },
        { displayName: "Team", propertyName: "team", isEditable: false },
        { displayName: "Category", propertyName: "category", isEditable: false }
    ];
    
    @ViewChild(FighterFilter) fighterFilter: FighterFilter;
    @ViewChild(FileUpload) fileUpload: FileUpload

    constructor(private dataService: DataService) {
    }

    

    //ng
    ngOnInit() {
    }

    ngAfterContentInit() {
    }


    //events
    onFilterChanged() {
        this.refreshTable();
    }

    
    uploadFile(file) {
        this.dataService.uploadFighterList(file);
    }

    private refreshTable() {
        this.dataService.getFigtersByFilter(this.fighterFilter.currentFilterValue).subscribe(data => this.fighters = data)
    }
}

