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
        let startIndex = (this.row - (Math.pow(2, k) - 1));
        let endIndex = (this.row - (Math.pow(2, k) - 1) - Math.pow(2, k + 1));
        let middleRange = (startIndex + endIndex) / 2;
        if (this.stage == 0 || this.stage == this.maxStage) {
           
            //0
            if (startIndex % freq == 0) {
                this.lowCorner(isRightSide);
            }
            //1
            
            if ((this.row - Math.pow(2,k)) % freq == 0) {
                this.vertLine(isRightSide);
            }
            //2
            if ( endIndex % freq == 0) {
                this.highCorner(isRightSide);
            }

        } else if ((this.stage == 1 || this.stage == this.maxStage - 1)) {
            //1
            if (startIndex % freq == 0) {
                this.lowCorner(isRightSide);
            }
            //3
            if ((this.row - Math.pow(2, k)) % freq == 0 || (this.row - Math.pow(2, k) - 1) % 8 == 0 || (this.row - Math.pow(2, k) -2) % 8 == 0 ) {
                this.vertLine(isRightSide);
            }
            //5
            if (endIndex % freq == 0) {
                this.highCorner(isRightSide);
            }
        } else if ((this.stage == 2 || this.stage == this.maxStage - 2)) {
            
            //3
            if (startIndex % freq == 0) {
                this.lowCorner(isRightSide);
            }
            //7
            for (var i = 0; i < 7; i++) {
                if ((this.row - 4 - i) % freq == 0) {
                    this.vertLine(isRightSide);
                }
            }
            //11
            if (endIndex % freq == 0) {
                this.highCorner(isRightSide);
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



}