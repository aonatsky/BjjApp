import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { BracketModel } from '../../core/model/bracket.models';
import './../../event-admin/event-management/brackets-generation/bracket-generation.component.scss';
import './bracket.component.scss';
import { ViewEncapsulation, OnChanges } from '@angular/core';
import { RoundModel } from '../../core/model/round.models';

@Component({
    selector: 'bracket',
    templateUrl: './bracket.component.html',
    encapsulation: ViewEncapsulation.None
})

export class BracketComponent implements OnChanges {

    @Input() bracket: BracketModel;
    roundGroups: RoundModel[][];
    maxStage: number = 0;
    rows: number[];
    columns: number[];

    constructor() {

    }

    ngOnChanges(changes: SimpleChanges): void {
        this.refreshBracketData();
    }

    private refreshBracketData() {
        this.maxStage = this.getMaxStage(this.bracket.roundModels.length);
        this.rows = this.getRows();
        this.roundGroups = this.getRoundsForStage();
        this.columns = this.getColumns();
    }

    private getRoundsForStage(): RoundModel[][] {
        let roundGroups: RoundModel[][] = [];
        for (var i = 0; i <= this.maxStage; i++) {
            roundGroups.push(this.bracket.roundModels.filter(r => r.stage == i));
        }
        console.log(roundGroups);
        return roundGroups;
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
        let colsCount = this.maxStage * 2 * 2;
        for (let i = 0; i <= colsCount; i++) {
            cols.push(i);
        }
        return cols;
    }

    private getRows(): number[] {
        let rows = [];
        for (let i = 0; i < this.bracket.roundModels.filter(r => r.stage === this.maxStage).length - 1; i++) {
            rows.push(i);
        }
        return rows;
    }


    displayData(col, row) {
        let maxCol = this.columns.length - 1;
        let centralCol = maxCol / 2;
        let isRightSide = col > centralCol;
        let depth = (isRightSide ? maxCol - col : col) / 2;
        let roundStage = this.maxStage - depth;
        let round: RoundModel;

        if (col === centralCol) {
            depth = depth - 1;
            roundStage = 0;
        }
        if (col % 2 == 0) {
            let shift = (Math.pow(2, depth) - 1);
            let freq = Math.pow(2, depth) + 1;
            let roundIndex = 0;
            if ((row - shift) % freq == 0) {
                roundIndex = (isRightSide ? this.roundGroups[roundStage].length/2 : 0) + ((row - shift) / freq);
                round = this.roundGroups[roundStage][roundIndex];
                return this.getRoundTemplate(round);
            } else {
                return `<div class="ui-g-12 table-block ui-g-nopad"></div>`;
            }

        } else {
            return `<div class="ui-g-1 inline-block" style=""></div>`;
        }

    }

    getRoundTemplate(round: RoundModel) {
        return `<div class="bracket ui-g-1 ui-g-nopad">
            <div class="bracket-participant-plate ui-g-12">
            ${round.firstParticipant ? round.firstParticipant.firstName + ' ' + round.firstParticipant.lastName : ''}
            </div>
            <div class="bracket-participant-plate ui-g-12">
            ${round.secondParticipant ? round.secondParticipant.firstName + ' ' + round.secondParticipant.lastName : ''}
            </div>
            </div>`;
    }

    getColumnClass(col) {
        if (col % 2 == 0) {
            return 'brackets-column';
        } else {
            return 'connector-column';
        }

    }
}