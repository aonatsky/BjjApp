
import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {Fighter} from '../../model/fighter.model'
import {WeightClass} from '../../model/weight-class.model'

@Injectable()
export abstract class DataService {
    public abstract getFigters(weightClass:string): Observable<Fighter[]>;
    public abstract getWeightClasses(): Observable<WeightClass[]>;

    
    
}