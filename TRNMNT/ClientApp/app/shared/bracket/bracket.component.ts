import { Component, OnInit, Input } from '@angular/core';
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
        this.maxStage = this.getMaxStage();
        this.columns = this.getColumns();
    }



    private getMaxStage(): number {
        let roundsCount = this.bracket.roundModels.filter(r => r.roundType == 0).length;
        for (let i = 0; i < 5; i++) {
            roundsCount -= Math.pow(2, i);
            if (roundsCount == 0) {
                return i;
            }
        }
    }

    private getColumns(): number[] {
        const cols = [];
        const maxColumn = this.maxStage * 2;
        for (let i = 0; i <= maxColumn; i++) {
            cols.push(i);
        }
        return cols;
    }



    private getRoundTemplate(round: RoundModel, isRightSide: boolean): string {
        const matchSide = isRightSide ? 'right-side' : '';
        let matchType = '';
        if (round.stage == 0) {
            matchType = round.roundType == 1 ? 'third-place' : 'final';
        }

        return `<div class="match ${matchSide} ${matchType} "><div class="match-content ui-g-1 ui-g-nopad">
            <div class="participant-plate ui-g-12">
            ${round.firstParticipant ? round.firstParticipant.firstName + ' ' + round.firstParticipant.lastName : ''}
            </div>
            <div class="participant-plate ui-g-12">
            ${round.secondParticipant ? round.secondParticipant.firstName + ' ' + round.secondParticipant.lastName : ''}
            </div>
            </div></div>`;
    }


    private isCentralCol(colNumber): boolean {
        return colNumber == (this.columns.length - 1) / 2;
    }

    private getMatchSideClass(colNumber): string {
        return colNumber > (this.columns.length - 1) / 2 ? 'right-side' : '';
    }

    private getMatchTypeClass(matchModel: RoundModel): string {
        let matchType = '';
        if (matchModel.stage == 0) {
            matchType = matchModel.roundType == 1 ? 'third-place' : 'final';
        }
        return matchType;
    }

    private getRoundModels(colNumber: number): RoundModel[] {
        const maxCol = this.columns.length - 1;
        const isRightSide = colNumber > maxCol / 2;
        const depth = (isRightSide ? maxCol - colNumber : colNumber);
        const stage = this.maxStage - depth;
        const models = this.bracket.roundModels.filter(r => r.stage == stage);
        if (stage === 0) {
            return models;
        } else {
            if (isRightSide) {
                return models.splice(models.length / 2, models.length);

            } else {
                return models.splice(0, models.length / 2);
            }
        }
    }

}

