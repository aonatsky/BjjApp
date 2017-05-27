import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { DataService } from '../contracts/data.service'
import { LoggerService } from '../../../core/services/logger.service';
import { Response, ResponseContentType } from '@angular/http'
import { ApiMethods } from '../consts/api-methods.consts'
import { HttpService } from '../http/http.service'
import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { Category } from "../../model/category.model";

@Injectable()
export class DataApiService extends DataService {


    constructor(private httpService: HttpService, private logger: LoggerService) {
        super()
    }

    //Categories
    public getCategories(): Observable<Category[]> {
        return this.httpService.get(ApiMethods.tournament.categories).map(r => this.httpService.getArray<Category>(r))
    }

    public addCategory(category: Category): Observable<any> {
        return this.httpService.post(ApiMethods.tournament.categories, category)
    }

    public updateCategory(category: Category): Observable<any> {
        return this.httpService.put(ApiMethods.tournament.categories, category)
    }

    public deleteCategory(category: Category): Observable<any> {
        return this.httpService.delete(ApiMethods.tournament.categories, category)
    }


    //Fighters
    public uploadFighterList(file: any): Observable<any> {
        return this.httpService.postFile(ApiMethods.tournament.fighters.uploadlist, file).map(data => this.httpService.getJson(data))
    }

    public getFigters(filter: FighterFilterModel): Observable<Fighter[]> {
        return this.httpService.get(ApiMethods.tournament.fighters.fighter).map(r => this.httpService.getArray<Fighter>(r))
    }

    public getFigtersByFilter(filter: FighterFilterModel): Observable<Fighter[]> {
        return this.httpService.post(ApiMethods.tournament.fighters.getByFilter, filter).map(r => this.httpService.getArray<Fighter>(r))
    }

    public deleteFighter(fighterId: string): Observable<any> {
        return this.httpService.deleteById(ApiMethods.tournament.fighters.fighter, fighterId);
    }
    //Brackets
    public getBracketsFile(filter: FighterFilterModel, fileName: string): Observable<void> {
        return this.httpService.post(ApiMethods.tournament.fighters.getBrackets, filter, ResponseContentType.Blob).map(r => this.httpService.getExcelFile(r, fileName))
    }

    //WeightDivisions

    public getWeightDivisions(): Observable<WeightDivision[]> {
        return this.httpService.get(ApiMethods.tournament.weightDivisions).map(r => this.httpService.getArray<WeightDivision>(r))
    }
    public addWeightDivision(weightDivision: WeightDivision): Observable<any> {
        return this.httpService.post(ApiMethods.tournament.weightDivisions, weightDivision)
    }
    public updateWeightDivision(weightDivision: WeightDivision): Observable<any> {
        return this.httpService.put(ApiMethods.tournament.weightDivisions, weightDivision)
    }
    public deleteWeightDivision(weightDivision: WeightDivision): Observable<any> {
        return this.httpService.delete(ApiMethods.tournament.weightDivisions, weightDivision)
    }

    //Static Content
    public getStaticContent(url): Observable<any> {
        return this.httpService.get(url); 
    }

}