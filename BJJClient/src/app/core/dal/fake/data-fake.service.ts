import {Injectable} from '@angular/core'
import {Observable} from 'rxjs/Observable';
import {DataService} from '../contracts/data.service'
import {Fighter} from '../../model/fighter.model'

@Injectable()
export class DataServiceFake extends DataService {
    public getFigters(weightClassName:string): Observable<Fighter[]>{
        //let fighters = new Fighter[];
        let fighters =  [
            new Fighter("Marcelo","Garcia","Alliance",80),
            new Fighter("Saulo","Ribeiro","Ribeiro JJ",80),
            new Fighter("Xandi","Ribeiro","Ribeiro JJ",90),
            new Fighter("Braulio","Estima","Gracie Barra",80),
            new Fighter("Victor","Estima","Gracie Barra",90),
            new Fighter("Lucas","Rocha","ZRTeam",80),
            new Fighter("Max","Carvalho","ZRTeam",90),
            new Fighter("Bruno","Malfacine","Alliance",60),
            new Fighter("Caio","Terra","CTA",60)
            ]
        return Observable.of(fighters);
    }
}