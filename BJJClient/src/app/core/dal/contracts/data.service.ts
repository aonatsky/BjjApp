import {BeltDivision} from '../../model/belt-division.model';
import {FighterFilterModel} from '../../model/fighter-filter.model';

import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {Fighter} from '../../model/fighter.model'
import {WeightDivision} from '../../model/weight-division.model'
import {AgeDivision} from '../../model/age-division.model'
import {Fight} from '../../model/fight.model'

@Injectable()
export abstract class DataService {
    public abstract getFigters(filter:FighterFilterModel): Observable<Fighter[]>
    public abstract getWeightDivisions(): Observable<WeightDivision[]>;
    public abstract getAgeDivisions(): Observable<AgeDivision[]>;
    public abstract getBeltDivisions(): Observable<BeltDivision[]>;
    public abstract getFights(fgihtListID:AAGUID) : Observable<Fight[]>
}