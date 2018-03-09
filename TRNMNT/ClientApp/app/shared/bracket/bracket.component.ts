﻿import { Component, OnInit, Input } from '@angular/core';
import { BracketModel } from '../../core/model/bracket.models';
import './../../event-admin/event-management/brackets-generation/bracket-generation.component.scss';
import './bracket.component.scss';
import { ViewEncapsulation } from '@angular/core';
import { RoundModel } from '../../core/model/round.models';

@Component({
    selector: 'bracket',
    templateUrl: './bracket.component.html',
    encapsulation: ViewEncapsulation.None
})

export class BracketComponent {
    @Input() bracket: BracketModel;
    maxStage: number = 0;
    columns: number[];

    constructor() {

    }

    ngOnInit() {
        this.maxStage = this.getMaxStage(this.bracket.roundModels.length);
        this.columns = this.getColumns();
    }



    private getMaxStage(roundsCount: number): number {
        for (let i = 0; i < 5; i++) {
            roundsCount -= Math.pow(2, i);
            if (roundsCount == 0) {
                return i;
            }
        }
    }

    private getColumns(): number[] {
        let cols = [];
        let maxColumn = this.maxStage * 2;
        for (let i = 0; i <= maxColumn; i++) {
            cols.push(i);
        }
        return cols;
    }



    private getRoundTemplate(round: RoundModel, isRightSide: boolean): string {
        let matchClass = isRightSide ? 'right-side' : '';
        return `<div class="match ${matchClass} "><div class="match-content ui-g-1 ui-g-nopad">
            <div class="participant-plate ui-g-12">
            ${round.firstParticipant ? round.firstParticipant.firstName + ' ' + round.firstParticipant.lastName : ''}
            </div>
            <div class="participant-plate ui-g-12">
            ${round.secondParticipant ? round.secondParticipant.firstName + ' ' + round.secondParticipant.lastName : ''}
            </div>
            </div></div>`;
    }





    private getRounds(colNumber: number) {
        let htmlData = '';
        let maxCol = this.columns.length - 1;
        let isRightSide = colNumber > maxCol / 2;
        let depth = (isRightSide ? maxCol - colNumber : colNumber);
        let stage = this.maxStage - depth;
        let count = this.bracket.roundModels.filter(r => r.stage == stage).length;
        if (stage == 0) {
            htmlData += '<div class="match"></div>';
            htmlData += this.getRoundTemplate(this.bracket.roundModels.filter(r => r.stage == stage)[0], false);
            htmlData += '<div class="match"></div>';
        } else {
            this.bracket.roundModels.filter(r => r.stage == stage).forEach((r, j) => {
                if (isRightSide) {
                    if (j >= count / 2) {
                        htmlData += this.getRoundTemplate(r, isRightSide);
                    }
                } else {
                    if (j < count / 2) {
                        htmlData += this.getRoundTemplate(r, isRightSide);
                    }
                }
            });
        }
        return htmlData;


    }

}

