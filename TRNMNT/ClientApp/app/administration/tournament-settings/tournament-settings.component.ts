import { Observable } from "rxjs/Observable";
import { WeightDivision } from '../../core/model/weight-division.model';
import { DataService } from '../../core/dal/contracts/data.service'
import { AfterViewInit, OnInit, Component } from '@angular/core';
import { Category } from "../../core/model/category.model";
import { CrudColumn } from "../../shared/crud/crud.component";
import { NotificationService } from '../../core/services/notification.service'
import { LoaderService } from '../../core/services/loader.service'
import { HttpService } from '../../core/dal/http/http.service';
import { RestService } from '../../core/dal/rest/rest.service';


@Component({
    selector: 'tournament-settings',
    templateUrl: './tournament-settings.component.html'
})


export class TournamentSettingsComponent implements OnInit {


    constructor(private dataService: DataService, private notificationService: NotificationService, private loaderService: LoaderService, private restService: RestService) {
    }


    ngOnInit(): void {
        this.loaderService.showLoader();
        Observable.forkJoin(this.restService.get<Category>(Category), this.dataService.getWeightDivisions())
            .subscribe(data => this.onDataLoad(data), () => this.notificationService.showGenericError())
    }


    private onDataLoad(data) {
        this.categories = data[0];
        this.weightDivisions = data[1];
        this.loaderService.hideLoader();

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
        this.restService.add<Category>(Category, category).subscribe(() => this.refreshCategories(), () => this.notificationService.showGenericError());
    }

    updateCategory(category: Category) {
        this.restService.update<Category>(Category, category).subscribe(() => this.refreshCategories(), () => this.notificationService.showGenericError());
    }

    deleteCategory(category: Category) {
        this.restService.delete<Category>(Category,category).subscribe(() => this.refreshCategories(), () => this.notificationService.showGenericError());
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
        this.dataService.addWeightDivision(category).subscribe(() => this.refreshWeightDivisions(), () => this.notificationService.showGenericError());
    }

    updateWeightDivision(category: WeightDivision) {
        this.dataService.updateWeightDivision(category).subscribe(() => this.refreshWeightDivisions(), () => this.notificationService.showGenericError());
    }

    deleteWeightDivision(category: WeightDivision) {
        this.dataService.deleteWeightDivision(category).subscribe(() => this.refreshWeightDivisions(), () => this.notificationService.showGenericError());
    }

    private test(data: any) {
        this.weightDivisions = data;
    }

}