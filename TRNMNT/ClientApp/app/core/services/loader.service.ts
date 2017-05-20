import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';


@Injectable()
export class LoaderService {
    public isLoaderShown = new BehaviorSubject<boolean>(false);

    public showLoader(): void {
        this.showHideLoader(true);
    }

    public hideLoader(): void {
        this.showHideLoader(false);
    }

    private showHideLoader(value: boolean) {
        this.isLoaderShown.next(value);
    }
}