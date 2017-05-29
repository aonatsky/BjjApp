import {CrudColumn} from '../../shared/crud/crud.component';
import { Component, OnInit, ViewChild, AfterViewInit, ViewEncapsulation, AfterContentInit, ElementRef } from '@angular/core';
import { DataService } from '../../core/dal/contracts/data.service'
import { Fighter } from '../../core/model/fighter.model'
import { FighterFilter } from '../../shared/fighter-filter/fighter-filter.component'
import { FileUpload } from '../../shared/file-upload/file-upload.component'
import { NotificationService } from '../../core/services/notification.service'
import { LoaderService } from '../../core/services/loader.service'

@Component({
    selector: 'fighterlist',
    templateUrl: './fighter-list.component.html',
    styleUrls: ['./fighter-list.component.css']
})


export class FighterListComponent {

    fighters: Fighter[];
    fighterColumns: CrudColumn[] = [
        { displayName: "First Name", propertyName: "firstName", isEditable: false },
        { displayName: "Last Name", propertyName: "lastName", isEditable: false },
        { displayName: "DOB", propertyName: "dateOfBirth", isEditable: false },
        { displayName: "Team", propertyName: "team", isEditable: false },
        { displayName: "Category", propertyName: "category", isEditable: false },
        { displayName: "Weight Division", propertyName: "weightDivision", isEditable: false }
    ];
    
    @ViewChild(FighterFilter) fighterFilter: FighterFilter;
    @ViewChild("bracketsButton") bracketsButton: ElementRef;

    private isLoaded: boolean = false;

    constructor(private dataService: DataService, private notificationService: NotificationService, private loaderService: LoaderService) {
    }

    ngOnInit(): void {
        this.loaderService.showLoader();
    }
      
    //events
    onFilterChanged() {
        this.loaderService.showLoader();
        this.loadData();
    }

    private deleteFighter(fighter: Fighter) {
        this.loaderService.showLoader();
        this.dataService.deleteFighter(fighter.fighterId).subscribe(() => this.loadData());
    }

    private getBracketsFile() {
        this.loaderService.showLoader();
        var url = this.dataService.getBracketsFile(this.fighterFilter.currentFilterValue, this.getBracketsFileName()).subscribe(() => this.loaderService.hideLoader());
    }

    private getBracketsFileName(): string {
        return "brackets.xlsx"
    }



 
    private loadData() {
        this.dataService.getFigtersByFilter(this.fighterFilter.currentFilterValue).subscribe(data => this.onDataLoaded(data), () => this.notificationService.showGenericError())
    }

    private onDataLoaded(data) {
        this.fighters = data;
        this.setBracketsButtonVisibility();
        this.loaderService.hideLoader();
    }

    private setBracketsButtonVisibility()
    {
        let btn = this.bracketsButton.nativeElement;
        btn["disabled"] = !(this.fighterFilter.currentFilterValue.categoryIds.length == 1 && this.fighterFilter.currentFilterValue.weightDivisionIds.length == 1 && this.fighters.length > 0);
    }
}

