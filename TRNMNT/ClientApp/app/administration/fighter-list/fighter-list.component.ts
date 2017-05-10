import {CrudColumn} from '../../shared/crud/crud.component';
import { Component, OnInit, ViewChild, AfterViewInit, ViewEncapsulation, AfterContentInit } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { FighterFilter } from '../../shared/fighter-filter/fighter-filter.component'
import { FileUpload } from '../../shared/file-upload/file-upload.component'
import { NotificationService} from '../../core/services/notification.service'


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

    constructor(private dataService: DataService, private notificationService: NotificationService) {
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


    getBracketsFile() {
        if (this.fighterFilter.currentFilterValue.categories.length == 1 && this.fighterFilter.currentFilterValue.weightDivisions.length == 1) {
            this.dataService.getBracketsFile(this.fighterFilter.currentFilterValue).subscribe(data => window.open(window.URL.createObjectURL(data)));
        } else {
            this.notificationService.showWarn("Warning", "Please specify weightdivision and category")
        }
        
    }


    private refreshTable() {
        this.dataService.getFigtersByFilter(this.fighterFilter.currentFilterValue).subscribe(data => this.fighters = data)
    }
}

