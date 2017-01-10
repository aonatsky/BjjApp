import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {DataService} from '../contracts/data.service'
import {Fighter} from '../../model/fighter.model'
import {WeightClass} from '../../model/weight-class.model'

@Injectable()
export class DataFakeService extends DataService {
    
    fighters =  [
            new Fighter("Marcelo","Garcia","Alliance",80),
            new Fighter("Saulo","Ribeiro","Ribeiro JJ",80),
            new Fighter("Xandi","Ribeiro","Ribeiro JJ",90),
            new Fighter("Braulio","Estima","Gracie Barra",80),
            new Fighter("Victor","Estima","Gracie Barra",90),
            new Fighter("Lucas","Rocha","ZRTeam",80),
            new Fighter("Max","Carvalho","ZRTeam",90),
            new Fighter("Bruno","Malfacine","Alliance",60),
            new Fighter("Caio","Terra","CTA",60)
            ];  
    
    weightClasses = [
        new WeightClass("Feather",60),
        new WeightClass("Middle",80),
        new WeightClass("Heavy",60)
    ]
    

    public getFigters(weightClass:string): Observable<Fighter[]>{
        return Observable.of(this.fighters.filter(f => f.weight == this.weightClasses.filter(w => w.name == weightClass)[0].weight));
    }

    public getWeightClasses(): Observable<WeightClass[]>{
        return Observable.of(this.weightClasses);
    }
}