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

    constructor() {

    }

    ngOnInit() {
        this.maxStage = this.getMaxStage();
        this.columns = this.getColumns();
    }



    private getMaxStage(): number {
        let roundsCount = this.bracket.roundModels.filter(r => r.roundType !== 1).length;
        for (let i = 0; i < 5; i++) {
            roundsCount -= Math.pow(2, i);
            if (roundsCount === 0) {
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

    private addEmptyMatch(colNumber: number): boolean {
        if (colNumber == (this.columns.length - 1) / 2) {
            return this.bracket.roundModels.length > 1;
        };
        return false;
    }

    private getMatchSideClass(colNumber): string {
        return colNumber > (this.columns.length - 1) / 2 ? 'right-side' : '';
    }

    private getMatchTypeClass(matchModel: RoundModel): string {
        let matchTypeClass = '';
        if (matchModel.stage == 0) {
            matchTypeClass = matchModel.roundType == 1 ? 'third-place' : 'final';
            if (this.bracket.roundModels.length === 1) {
                matchTypeClass += ' only-final';
            }
        }
        return matchTypeClass;
    }

    private getRoundModels(colNumber: number): RoundModel[] {
        const maxCol = this.columns.length - 1;
        const isRightSide = colNumber > maxCol / 2;
        const depth = (isRightSide ? maxCol - colNumber : colNumber);
        const stage = this.maxStage - depth;
        const models = this.bracket.roundModels.filter(r => r.stage == stage).sort((r1, r2) => { return r1.order - r2.order });
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

    private displayParticipantData(model: RoundModel, participantNumber: number) {
        if (participantNumber == 1) {
            return model.AParticipant
                ? model.AParticipant.firstName + ' ' + model.AParticipant.lastName
                : '';
        } else {
            if (model.BParticipant) {
                return model.BParticipant.firstName + ' ' + model.BParticipant.lastName;
            } else if (model.roundType == 2) {
                return 'LOST FIRST ROUND';
            } else {
                return '';
            }
        }
    }

    private displayRoundResult(model: RoundModel, participantNumber: number) {
        if (participantNumber === 1) {
            return model.AParticipantResult;
        }
    }

    private onRoundClick(model: RoundModel) {
        if (this.isEditable(model)) {
            this.roundClick.emit(model);
        }
    }

    private getWinnerClass(model: RoundModel) {
        if (model.winnerParticipant) {
            if (!!model.AParticipant && model.winnerParticipant.participantId === model.AParticipant.participantId) {
                return 'winner-1';
            } else {
                return 'winner-2';
            }
        } else {
            return '';
        }
    }

    private isEditable(model: RoundModel): boolean {
        if (model.AParticipant && model.BParticipant) {
            if (model.winnerParticipant && model.nextRoundId) {
                const nextRound = this.bracket.roundModels.filter(r => r.roundId === model.nextRoundId)[0];
                if (nextRound && !nextRound.winnerParticipant) {
                    return true;
                }
                return false;
            }

            return true;
        } else {
            return false;
        }
    }
}

