import { DataService } from '../../core/dal/contracts/data.service'
import { AfterViewInit, OnInit, Component } from '@angular/core';
import { Category } from "../../core/model/category.model";
import { CrudColumn } from "../../shared/crud/crud.component";


@Component({
    selector: 'tournament-settings',
    templateUrl: './tournament-settings.component.html'
})


export class TournamentSettingsComponent implements OnInit {

    categoryColumns: CrudColumn[] = [
        { displayName: "Id", propertyName: "categoryId", isEditable: false },
        { displayName: "Name", propertyName: "name", isEditable: true }
    ];

    ngOnInit(): void {
        this.refreshCategories();
    }


    refreshCategories() {
        this.dataService.getCategories().subscribe(data => this.categories = data)
    }


    categories: Category[];
    constructor(private dataService: DataService) {

    }

    addCategory(category: Category) {
        this.dataService.addCategory(category).subscribe(() => this.refreshCategories());
    }

    updateCategory(category: Category) {
        this.dataService.updateCategory(category).subscribe(() => this.refreshCategories());
    }

    deleteCategory(category: Category) {
        this.dataService.deleteCategory(category).subscribe(() =>  this.refreshCategories());
    }




}