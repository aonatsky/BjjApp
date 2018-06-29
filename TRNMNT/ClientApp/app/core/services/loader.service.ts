import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


@Injectable()
export class LoaderService {
    loaderCounter: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    showLoader(): void {
        this.displayLoader(true);
    }

    hideLoader(): void {
        this.displayLoader(false);
    }

    displayLoader(value: boolean) {
        let counter = value ? this.loaderCounter.value + 1 : this.loaderCounter.value - 1;
        if (counter < 0) {
            counter = 0;
        }
        this.loaderCounter.next(counter);
    }
}