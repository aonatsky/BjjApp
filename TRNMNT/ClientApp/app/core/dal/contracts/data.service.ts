
import { CategoryWithDivisionFilterModel as FighterFilterModel } from '../../model/category-with-division-filter.model';

import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';

import { WeightDivisionModel } from '../../model/weight-division.models'
import { CategoryModel } from "../../model/category.models";

@Injectable()
export abstract class DataService {
    abstract deleteFighter(fighterId: string): Observable<any>;
    abstract getWeightDivisions(): Observable<WeightDivisionModel[]>;
    abstract uploadFighterList(file: any): Observable<any>;
    abstract getBracketsFile(filter: FighterFilterModel, fileName: string): Observable<void>
    abstract getCategories(): Observable<CategoryModel[]>;
    abstract addCategory(category: CategoryModel): Observable<any>;
    abstract updateCategory(category: CategoryModel): Observable<any>;
    abstract deleteCategory(category: CategoryModel): Observable<any>;
    abstract addWeightDivision(weightDivision: WeightDivisionModel): Observable<any>;
    abstract updateWeightDivision(weightDivision: WeightDivisionModel): Observable<any>;
    abstract deleteWeightDivision(weightDivision: WeightDivisionModel): Observable<any>;
    abstract getStaticContent(url): Observable<any>

}