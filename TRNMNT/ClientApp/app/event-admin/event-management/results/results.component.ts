import { Component, OnInit } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';
import { Input } from '@angular/core';
import {CategorySimpleModel} from '../../../core/model/category.models';
import {CategoryService} from '../../../core/services/category.service';

@Component({
    selector: 'results',
    templateUrl: 'results.component.html',
    encapsulation: ViewEncapsulation.None
})

export class ResultsComponent {
    @Input() eventId: string;
    private categories: CategorySimpleModel[] = [];
    private selectedCategories: CategorySimpleModel[] = [];

    constructor(private categoryService: CategoryService) {

    }

    ngOnInit() {
        this.categoryService.getCategoriesByEventId(this.eventId).subscribe(r => {
            this.categories = r;
        });

    }


}

