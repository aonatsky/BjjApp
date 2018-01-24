import { Component } from '@angular/core';
import { ViewEncapsulation, Input } from '@angular/core';
import './connector.component.scss';

@Component({
    selector: 'connector',
    encapsulation: ViewEncapsulation.None,
    template: `<div class="ui-g-12 ui-g-nopad {{topClass}}"></div><div class="ui-g-12 ui-g-nopad {{bottomClass}}"></div>`

})

export class ConnectorComponent {
    @Input() stage: number;
    @Input() row: number;
    @Input() maxStage: number;

    topClass: string = '';
    bottomClass: string = '';


    ngOnInit() {
        let centerStage = this.maxStage / 2;
        let isRightSide = this.stage > centerStage;
        let k = isRightSide ? this.maxStage - this.stage : this.stage;
        let freq = Math.pow(2, 2 + k);
        let startShift = (Math.pow(2, k) - 1);
        let endShift = ((Math.pow(2, k) - 1) + Math.pow(2, k + 1));
        let startIndex = (this.row - startShift);
        let endIndex = (this.row - endShift);
        let middleRange = (startShift + endShift) / 2;
        if (this.stage != centerStage + 0.5 && this.stage != centerStage - 0.5) {
            if (startIndex % freq == 0) {
                this.lowCorner(isRightSide);
            }
            for (let j = 0; j < middleRange; j++) {
                if ((this.row - Math.pow(2, k) - j) % freq == 0) {
                    this.vertLine(isRightSide);
                }
            }
            if (endIndex % freq == 0) {
                this.highCorner(isRightSide);
            }
        } else {
            if (startIndex % freq == 0) {
                this.straightLine();
            }
        }
    }

    private lowCorner(isRightSide: boolean) {
        this.topClass += ' border-bottom';
        this.bottomClass += isRightSide ? ' border-left' : ' border-right';
    }

    private highCorner(isRightSide: boolean) {
        this.topClass += ' border-bottom';
        this.topClass += isRightSide ? ' border-left' : ' border-right';
    }

    private vertLine(isRightSide: boolean) {
        this.bottomClass += isRightSide ? ' border-left' : ' border-right';
        this.topClass += isRightSide ? ' border-left' : ' border-right';
    }

    private straightLine() {
        this.topClass += ' border-bottom';
    }



}