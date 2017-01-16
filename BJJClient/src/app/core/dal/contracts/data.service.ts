
import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {Fighter} from '../../model/fighter.model'
import {WeightClass} from '../../model/weight-class.model'
import {Fight} from '../../model/fight.model'

@Injectable()
export abstract class DataService {
    public abstract getFigters(weightClass:string): Observable<Fighter[]>;
    public abstract getWeightClasses(): Observable<WeightClass[]>;
    public abstract getFights(fgihtListID:AAGUID) : Observable<Fight[]>
}