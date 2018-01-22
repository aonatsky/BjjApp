import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';


@Injectable()
export class LoaderService {
    public loaderCounter: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    public showLoader(): void {
        debugger;
        this.displayLoader(true);
    }

    public hideLoader(): void {
        this.displayLoader(false);
    }

    public displayLoader(value: boolean) {
        let counter = value ? this.loaderCounter.value + 1 : this.loaderCounter.value - 1;
        if (counter < 0) {
            counter = 0;
        }
        this.loaderCounter.next(counter);
    }
}