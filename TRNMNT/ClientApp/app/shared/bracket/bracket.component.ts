import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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

export class BracketComponent implements OnInit {
    @Input() bracket: BracketModel;
    @Output() roundClick: EventEmitter<RoundModel> = new EventEmitter();
    maxStage: number = 0;
    columns: number[];

    selectedRoundDetails: RoundModel;
    showRoundPanel: boolean = false;

    constructor() {

    }

    ngOnInit() {
        this.maxStage = this.getMaxStage();
        this.columns = this.getColumns();
        this.selectedRoundDetails = this.bracket.roundModels[0];
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

    private onRoundClick(model:RoundModel) {
        this.roundClick.emit(model);
    }

}

