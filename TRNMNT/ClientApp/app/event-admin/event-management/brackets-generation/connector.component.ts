import { Component } from '@angular/core';
import { ViewEncapsulation, Input } from '@angular/core';
import './connector.component.scss';

@Component({
    selector: 'connector',
    encapsulation: ViewEncapsulation.None,
    template: `<div class="ui-g-12 ui-g-nopad {{topClass}}"></div><div class="ui-g-12 ui-g-nopad {{bottomClass}}"></div>`

})

export class ConnectorComponent {
    @Input() round: number;
    @Input() row: number;
    @Input() maxRound: number;

    topClass: string = '';
    bottomClass: string = '';


    ngOnInit() {
        const centerRound = this.maxRound / 2;
        const isRightSide = this.round > centerRound;
        const k = isRightSide ? this.maxRound - this.round : this.round;
        const freq = Math.pow(2, 2 + k);
        const startShift = (Math.pow(2, k) - 1);
        const endShift = ((Math.pow(2, k) - 1) + Math.pow(2, k + 1));
        const startIndex = (this.row - startShift);
        const endIndex = (this.row - endShift);
        const middleRange = (startShift + endShift) / 2;
        if (this.round != centerRound + 0.5 && this.round != centerRound - 0.5) {
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