import { DataService } from '../../core/dal/contracts/data.service'
import { AfterViewInit, OnInit, Component } from '@angular/core';
import { Category } from "../../core/model/category.model";
import { CrudColumn } from "../../shared/crud/crud.component";


@Component({
    selector: 'tournament-settings',
    templateUrl: './tournament-settings.component.html',
    styles: [`.ui-grid-row div{
        padding: 4px 10px;
    }`]
})


export class TournamentSettingsComponent implements OnInit {

    displayDialog: boolean;
    newEntity: boolean;
    selectedEntity : any;
    category: Category = new Category();
    categoryColumns: CrudColumn[] = [{displayName: "ID",propertyName:"categoryId"},{displayName: "Name",propertyName:"name"}]

    ngOnInit(): void {
        this.dataService.getCategories().subscribe(data => this.categories = data)
    }


    showDialogToAdd() {
        this.newEntity = true;
        this.category = new Category();
        this.displayDialog = true;
    }


    categories: Category[];
    constructor(private dataService: DataService) {

    }

    save() {
        this.displayDialog = false;
    }

    delete() {
        this.displayDialog = false;
    }

    onRowSelect(event) {
        this.showDialogToAdd();
    }

    cloneCar(c: Category): Category {
        let category = new Category();
        for (let prop in c) {
            category[prop] = c[prop];
        }
        return category;
    }



}