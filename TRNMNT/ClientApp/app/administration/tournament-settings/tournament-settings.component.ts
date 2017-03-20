import { DataService } from '../../core/dal/contracts/data.service'
import { AfterViewInit, OnInit, Component } from '@angular/core';
import { Category } from "../../core/model/category.model";


@Component({
    selector: 'tournament-settings',
    template: `<p-dataTable [value]="categories">
    <p-column field="categoruId" header="Vin"></p-column>
    <p-column field="name" header="Year"></p-column>
</p-dataTable>`
})


export class TournamentSettingsComponent implements OnInit {
    
    isInit: boolean = false;
    
    ngOnInit(): void {
        this.dataService.getCategories().subscribe(data => this.categories = data)
        this.isInit = true;
    }


    categories: Category[];

    constructor(private dataService: DataService) {

    }

}