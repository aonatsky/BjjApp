import { DataService } from '../../core/dal/contracts/data.service'
import { AfterViewInit, OnInit, Component } from '@angular/core';
import { Category } from "../../core/model/category.model";

@Component({
    selector: 'tournament-settings',
    template: `<div *ngIf="categories">{{categories[0].name}}</div>`
})


export class TournamentSettingsComponent implements OnInit {
        ngOnInit(): void {
            this.dataService.getCategories().subscribe(data => this.categories = data)
        }

categories : Category[];

constructor(private dataService:DataService) {
    
}

}