import { BeltDivision } from '../../model/belt-division.model';
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { DataService } from '../contracts/data.service'
import { LoggerService } from '../../../core/services/logger.service';

import { ApiMethods } from '../consts/api-methods.consts'
import { ApiServer } from '../servers/api.server'


import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { AgeDivision } from '../../model/age-division.model'
import { Fight } from '../../model/fight.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { Category } from "../../model/category.model";

@Injectable()
export class DataApiService extends DataService {
    constructor(private apiServer: ApiServer, private logger: LoggerService) {
        super()
    }


    public getCategories(): Observable<Category[]> {
        return this.apiServer.get(ApiMethods.tournament.categories).map(r => this.getArray<Category>(r))
    }

    private getArray<T>(result: any):T[] {
        if (result == {}) {
            let arrat = T[];
            return T[];
        }
        return ;
    }

    public addCategory(category: Category): any {
        return this.apiServer.post(ApiMethods.tournament.categories, category)
    }

    public updateCategory(category: Category): any {
        return this.apiServer.put(ApiMethods.tournament.categories, category)
    }

    public deleteCategory(category: Category): any {
        return this.apiServer.delete(ApiMethods.tournament.categories, category)
    }

    public uploadFighterList(file: any): Observable<any> {
        throw new Error('Method not implemented.');
    }

    public getFigters(filter: FighterFilterModel): Observable<Fighter[]> {
        return this.apiServer.get(ApiMethods.tournament.fighters)
    }


    private handleErrorResponse(response: any): Observable<any> {
        let data: any = response;
        return Observable.throw(data);
    }

    public getWeightDivisions(): Observable<WeightDivision[]> {
        return this.apiServer.get(ApiMethods.tournament.fighters)
    }

    public getFights(fightListID: AAGUID): Observable<Fight[]> {
        return this.apiServer.get(ApiMethods.tournament.ageDivisions)
    }

    public getAgeDivisions(): Observable<AgeDivision[]> {
        return this.apiServer.get(ApiMethods.tournament.ageDivisions)
    }
    public getBeltDivisions(): Observable<BeltDivision[]> {
        return this.apiServer.get(ApiMethods.tournament.ageDivisions)
    }
}