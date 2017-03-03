
import { BeltDivision } from '../../model/belt-division.model';
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { DataService } from '../contracts/data.service'
import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { AgeDivision } from '../../model/age-division.model'
import { Fight } from '../../model/fight.model'

@Injectable()
export class DataFakeService extends DataService {
       
       public uploadFighterList(file: any): Observable<any> {
            return Observable.of({result:"fake"})
        }


    fighters = [
        new Fighter("Marcelo", "Garcia", "Alliance", 80, 37, "Elite"),
        new Fighter("Saulo", "Ribeiro", "Ribeiro JJ", 80, 37, "Elite"),
        new Fighter("Xandi", "Ribeiro", "Ribeiro JJ", 90, 33, "Elite"),
        new Fighter("Braulio", "Estima", "Gracie Barra", 80, 25, "Elite"),
        new Fighter("Victor", "Estima", "Gracie Barra", 90, 37, "Blue"),
        new Fighter("Lucas", "Rocha", "ZRTeam", 80, 37, "White"),
        new Fighter("Max", "Carvalho", "ZRTeam", 90, 25, "Blue"),
        new Fighter("Bruno", "Malfacine", "Alliance", 60, 27, "White"),
        new Fighter("Caio", "Terra", "CTA", 60, 16, "White")
    ];

    weightDivisions = [
        new WeightDivision(0, "Light", 60),
        new WeightDivision(1, "Middle", 80),
        new WeightDivision(2, "Heavy", 90)
    ]

    fights = [
        new Fight(this.fighters[0], this.fighters[1]),
        new Fight(this.fighters[2], this.fighters[3]),
        new Fight(this.fighters[4], this.fighters[5]),
        new Fight(this.fighters[6], this.fighters[7])
    ];

    ageDivisions = [
        new AgeDivision(0, "Juvenile", 14),
        new AgeDivision(1, "Adult", 18),
        new AgeDivision(2, "Veteran", 35),
    ]

    beltDivisions = [
        new BeltDivision(0, "White"),
        new BeltDivision(1, "Blue"),
        new BeltDivision(2, "Elite"),
    ]

    public getFigters(filter: FighterFilterModel): Observable<Fighter[]> {
        return Observable.of(this.fighters.filter(f => this.checkWeight(filter.weightDivisions, f) && this.checkAge(filter.ageDivisions, f) && this.checkBelt(filter.beltDivisions, f)));

    }

    private checkWeight(weightDivisions: WeightDivision[], fighter: Fighter): boolean {
        if (weightDivisions.length > 1) {
            return true;
        } else {
            return fighter.weight == weightDivisions[0].weight;
        };
    }

    private checkAge(ageDivision: AgeDivision[], fighter: Fighter): boolean {
        if (ageDivision.length > 1) {
            return true;
        } else {
            return fighter.age > ageDivision[0].age;
        }
    }

    private checkBelt(beltDivision: BeltDivision[], fighter: Fighter): boolean {
        if (beltDivision.length > 1) {
            return true;
        } else {
            return fighter.belt == beltDivision[0].name;
        };

    }






    public getWeightDivisions(): Observable<WeightDivision[]> {
        return Observable.of(this.weightDivisions);
    }

    public getFights(fgihtListID: AAGUID): Observable<Fight[]> {
        return Observable.of(this.fights);
    }

    public getBeltDivisions(): Observable<BeltDivision[]> {
        return Observable.of(this.beltDivisions);
    }

    public getAgeDivisions(): Observable<AgeDivision[]> {
        return Observable.of(this.ageDivisions);
    }
}