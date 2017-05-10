import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { DataService } from '../contracts/data.service'
import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { Category } from "../../model/category.model";

@Injectable()
export class DataFakeService extends DataService {


    public getFigtersByFilter(filter: FighterFilterModel): Observable<Fighter[]> {
        throw new Error('Method not implemented.');
    }

    public getBracketsFile(filter: FighterFilterModel): Observable<File> {
        throw new Error('Method not implemented.');
    }

    public addWeightDivision(weightDivision: WeightDivision) {
        throw new Error('Method not implemented.');
    }
    public updateWeightDivision(weightDivision: WeightDivision) {
        throw new Error('Method not implemented.');
    }
    public deleteWeightDivision(weightDivision: WeightDivision) {
        throw new Error('Method not implemented.');
    }


    constructor() {
        super();
    }

    public addCategory(category: Category) {
        throw new Error('Method not implemented.');
    }
    public updateCategory(category: Category) {
        throw new Error('Method not implemented.');
    }
    public deleteCategory(category: Category) {
        throw new Error('Method not implemented.');
    }


    public getCategories(): Observable<Category[]> {
        return Observable.of(this.categories);
    }



    public uploadFighterList(file: any): Observable<any> {
        return Observable.of(this.fighters);
    }


    fighters = [
        new Fighter("4c571d9a-3398-4677-831d-3373d270ec11", "Marcelo", "Garcia", "Alliance", "1986-10-10", "Elite"),
    ];

    weightDivisions = [
        new WeightDivision("4c571d9a-3398-4677-831d-3373d270ec11", "Light", 60),
        new WeightDivision("1e3602fc-687e-4ee7-a4a4-6202b3c0af54", "Middle", 80),
        new WeightDivision("1e3602fc-687e-4ee7-a4a4-6202b3c0af54", "Heavy", 90)
    ]



    categories = [
        new Category("4c571d9a-3398-4677-831d-3373d270ec12", "kids")
    ]

    public getFigters(filter: FighterFilterModel): Observable<Fighter[]> {
        return Observable.of(this.fighters);

    }
    public getWeightDivisions(): Observable<WeightDivision[]> {
        return Observable.of(this.weightDivisions);
    }
}