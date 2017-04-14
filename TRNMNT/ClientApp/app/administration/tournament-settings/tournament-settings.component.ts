import { WeightDivision } from '../../core/model/weight-division.model';
import { DataService } from '../../core/dal/contracts/data.service'
import { AfterViewInit, OnInit, Component } from '@angular/core';
import { Category } from "../../core/model/category.model";
import { CrudColumn } from "../../shared/crud/crud.component";


@Component({
    selector: 'tournament-settings',
    templateUrl: './tournament-settings.component.html'
})


export class TournamentSettingsComponent implements OnInit {


    constructor(private dataService: DataService) {
    }


    ngOnInit(): void {
        this.refreshCategories();
        this.refreshWeightDivisions();
    }


    //Categories    
    refreshCategories() {
        this.dataService.getCategories().subscribe(data => this.categories = data)
    }

    categories: Category[];

    categoryColumns: CrudColumn[] = [
        { displayName: "Id", propertyName: "categoryId", isEditable: false },
        { displayName: "Name", propertyName: "name", isEditable: true }
    ];

    addCategory(category: Category) {
        this.dataService.addCategory(category).subscribe(() => this.refreshCategories());
    }

    updateCategory(category: Category) {
        this.dataService.updateCategory(category).subscribe(() => this.refreshCategories());
    }

    deleteCategory(category: Category) {
        this.dataService.deleteCategory(category).subscribe(() => this.refreshCategories());
    }


    //WeightDivisions    
    refreshWeightDivisions() {
        this.dataService.getWeightDivisions().subscribe(data => this.test(data))
    }

    weightDivisionColumns: CrudColumn[] = [
        { displayName: "Id", propertyName: "weightDivisionId", isEditable: false },
        { displayName: "Name", propertyName: "name", isEditable: true }
    ];

    weightDivisions: WeightDivision[];


    addWeightDivision(category: WeightDivision) {
        this.dataService.addWeightDivision(category).subscribe(() => this.refreshWeightDivisions());
    }

    updateWeightDivision(category: WeightDivision) {
        this.dataService.updateWeightDivision(category).subscribe(() => this.refreshWeightDivisions());
    }

    deleteWeightDivision(category: WeightDivision) {
        this.dataService.deleteWeightDivision(category).subscribe(() => this.refreshWeightDivisions());
    }

    private test(data: any){
        this.weightDivisions = data;
    }

}