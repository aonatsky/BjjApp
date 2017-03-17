import {BeltDivision} from '../../model/belt-division.model';
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
            return this.apiServer.get(ApiMethods.tournament.categories)
            .map(response => { return response })
            .catch(errorResponse => this.handleErrorResponse(errorResponse));
        }

        private processResponse(response : any) : any{
            
        }

       
        public uploadFighterList(file: any): Observable<any> {
            throw new Error('Method not implemented.');
        }
    
    public getFigters(filter:FighterFilterModel): Observable<Fighter[]> {
        return this.apiServer.get(ApiMethods.tournament.fighters)
            .map(response => { return response.data })
            .catch(errorResponse => this.handleErrorResponse(errorResponse));
    }


    private handleErrorResponse(response:  any): Observable<any> {
        let data: any =  response;
        return Observable.throw(data);
    }

    public getWeightDivisions(): Observable<WeightDivision[]> {
         return this.apiServer.get(ApiMethods.tournament.fighters)
            .map(response => { return response.data })
            .catch(errorResponse => this.handleErrorResponse(errorResponse));
    }

    public getFights(fightListID: AAGUID): Observable<Fight[]> {
         return this.apiServer.get(ApiMethods.tournament.ageDivisions)
            .map(response => { return response.data })
            .catch(errorResponse => this.handleErrorResponse(errorResponse));
    }
    
    public getAgeDivisions(): Observable<AgeDivision[]> {
         return this.apiServer.get(ApiMethods.tournament.ageDivisions)
            .map(response => { return response.data })
            .catch(errorResponse => this.handleErrorResponse(errorResponse));
    }
    public getBeltDivisions(): Observable<BeltDivision[]> {
         return this.apiServer.get(ApiMethods.tournament.ageDivisions)
            .map(response => { return response.data })
            .catch(errorResponse => this.handleErrorResponse(errorResponse));
    }
}