
import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {Fighter} from '../../model/fighter.model'

@Injectable()
export abstract class DataService {
    public abstract getFigters(weightClassName:string): Observable<Fighter[]>;
}