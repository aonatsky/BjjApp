
import { CategoryWithDivisionFilterModel as FighterFilterModel } from '../../model/category-with-division-filter.model';

import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';

import { WeightDivisionModel } from '../../model/weight-division.models'
import { CategoryModel } from "../../model/category.models";

@Injectable()
export abstract class DataService {
    public abstract deleteFighter(fighterId: string): Observable<any>;
    public abstract getWeightDivisions(): Observable<WeightDivisionModel[]>;
    public abstract uploadFighterList(file: any): Observable<any>;
    public abstract getBracketsFile(filter: FighterFilterModel, fileName: string): Observable<void>
    public abstract getCategories(): Observable<CategoryModel[]>;
    public abstract addCategory(category: CategoryModel): Observable<any>;
    public abstract updateCategory(category: CategoryModel): Observable<any>;
    public abstract deleteCategory(category: CategoryModel): Observable<any>;
    public abstract addWeightDivision(weightDivision: WeightDivisionModel): Observable<any>;
    public abstract updateWeightDivision(weightDivision: WeightDivisionModel): Observable<any>;
    public abstract deleteWeightDivision(weightDivision: WeightDivisionModel): Observable<any>;
    public abstract getStaticContent(url): Observable<any>

}