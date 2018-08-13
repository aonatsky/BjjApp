import { Injectable } from '@angular/core';
import { HttpService } from '../dal/http/http.service';
import { FederationModel } from '../model/federation.model';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';

@Injectable()
export class FederationService {
  constructor(private httpService: HttpService) {}

  getFederation(): Observable<FederationModel> {
    return this.httpService.get<FederationModel>(ApiMethods.federation.getFederation);
  }

  updateFederation(model: FederationModel): Observable<any> {
    return this.httpService.post<FederationModel>(ApiMethods.federation.updateFederation, model);
  }
}
