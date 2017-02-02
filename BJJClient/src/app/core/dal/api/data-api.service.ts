import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {DataService} from '../contracts/data.service'
import {Fighter} from '../../model/fighter.model'
import {WeightDivision} from '../../model/weight-division.model'
import {Fight} from '../../model/fight.model'

@Injectable()
export class DataApiService extends DataService {

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
        new WeightDivision("Light",60),
        new WeightDivision("Middle",80),
        new WeightDivision("Heavy",90)
    ]
    
    fights = [
        new Fight(this.fighters[0],this.fighters[1]),
        new Fight(this.fighters[2],this.fighters[3]),
        new Fight(this.fighters[4],this.fighters[5]),
        new Fight(this.fighters[6],this.fighters[7])
    ]

    public getFigters(weightClass:string | null): Observable<Fighter[]>{
        if (weightClass != null) {
            return Observable.of(this.fighters.filter(f => f.weight == this.weightClasses.filter(w => w.name == weightClass)[0].weight));    
        }else{
            return Observable.of(this.fighters);
        }
        
    }
    
    // public getFigters(): Observable<Fighter[]>{
    //     return Observable.of(this.fighters);
    // }

    public getWeightClasses(): Observable<WeightDivision[]>{
        return Observable.of(this.weightClasses);
    }

    public getFights(fgihtListID:AAGUID):Observable<Fight[]>{
        return Observable.of(this.fights);
    }
}