import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { DataService } from '../contracts/data.service'
import { LoggerService } from '../../../core/services/logger.service';
import { Response, ResponseContentType } from '@angular/http'

import * as FileSaver from 'file-saver';


import { ApiMethods } from '../consts/api-methods.consts'
import { ApiServer } from '../servers/api.server'


import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { Category } from "../../model/category.model";

@Injectable()
export class DataApiService extends DataService {


    constructor(private apiServer: ApiServer, private logger: LoggerService) {
        super()
    }

    //Common
    private getArray<T>(response: any): T[] {
        let result = response.json();
        if (result.length == 0) {
            return [];
        }
        return result;
    }

    private handleErrorResponse(response: any): Observable<any> {
        let data: any = response;
        return Observable.throw(data);
    }

    private getResult(response: any) {
        return response.json();
    }

    private getExcelFile(response: Response): void {
        var blob = response.blob();
        FileSaver.saveAs(blob, response.headers.get("filename"));
    }



    //Categories
    public getCategories(): Observable<Category[]> {
        return this.apiServer.get(ApiMethods.tournament.categories).map(r => this.getArray<Category>(r))
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


    //Fighters
    public uploadFighterList(file: any): Observable<any> {
        return this.apiServer.postFile(ApiMethods.tournament.fighters.uploadlist, file).map(data => this.getResult(data))
    }

    public getFigters(filter: FighterFilterModel): Observable<Fighter[]> {
        return this.apiServer.get(ApiMethods.tournament.fighters.getAll).map(r => this.getArray<Fighter>(r))
    }

    public getFigtersByFilter(filter: FighterFilterModel): Observable<Fighter[]> {
        return this.apiServer.post(ApiMethods.tournament.fighters.getByFilter, filter).map(r => this.getArray<Fighter>(r))
    }

    //Brackets

    public getBracketsFile(filter: FighterFilterModel): Observable<void> {
        return this.apiServer.post(ApiMethods.tournament.fighters.getBrackets, filter, ResponseContentType.Blob).map(r => this.getExcelFile(r))
    }

    //WeightDivisions

    public getWeightDivisions(): Observable<WeightDivision[]> {
        return this.apiServer.get(ApiMethods.tournament.weightDivisions).map(r => this.getArray<WeightDivision>(r))
    }
    public addWeightDivision(weightDivision: WeightDivision) {
        return this.apiServer.post(ApiMethods.tournament.weightDivisions, weightDivision)
    }
    public updateWeightDivision(weightDivision: WeightDivision) {
        return this.apiServer.put(ApiMethods.tournament.weightDivisions, weightDivision)
    }
    public deleteWeightDivision(weightDivision: WeightDivision) {
        return this.apiServer.delete(ApiMethods.tournament.weightDivisions, weightDivision)
    }
}