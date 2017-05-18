
import { FighterFilterModel } from '../../model/fighter-filter.model';

import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { Category } from "../../model/category.model";

@Injectable()
export abstract class DataService {
    public abstract getFigters(filter: FighterFilterModel): Observable<Fighter[]>
    public abstract getFigtersByFilter(filter: FighterFilterModel): Observable<Fighter[]>
    public abstract deleteFighter(fighterId: string): Observable<any>;
    public abstract getWeightDivisions(): Observable<WeightDivision[]>;
    public abstract uploadFighterList(file: any): Observable<any>;
    public abstract getBracketsFile(filter: FighterFilterModel): Observable<void>
    public abstract getCategories(): Observable<Category[]>;
    public abstract addCategory(category: Category): Observable<any>;
    public abstract updateCategory(category: Category): Observable<any>;
    public abstract deleteCategory(category: Category): Observable<any>;
    public abstract addWeightDivision(weightDivision: WeightDivision): Observable<any>;
    public abstract updateWeightDivision(weightDivision: WeightDivision): Observable<any>;
    public abstract deleteWeightDivision(weightDivision: WeightDivision): Observable<any>;

}