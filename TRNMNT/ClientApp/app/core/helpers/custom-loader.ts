import { Injectable, Injector } from '@angular/core';
import { TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs';
import * as en from './../../../locale/en';
import * as ua from './../../../locale/ua';

@Injectable()
export class CustomLoader implements TranslateLoader {
  constructor(private http: HttpClient) {}

  getTranslation(lang: string): Observable<any> {
    switch(lang) { 
      case 'ua': { 
        return of(ua.data);
        break;
      } 
      case 'en': { 
         return of(en.data)
         break;
      } 
      default: { 
        return of(en.data)
        break;
      } 
    }
  }
}
