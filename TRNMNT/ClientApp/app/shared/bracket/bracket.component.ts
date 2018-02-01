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
    rounds: RoundModel[] = [];
    maxStage: number = 0;
    rows: number[];
    columns: number[];
    constructor() {

    }

    ngOnInit() {
        this.rounds = this.bracket.roundModels;
        this.maxStage = this.getMaxStage(this.rounds.length);
        console.log(this.rounds.length);
        console.log(this.maxStage);
        this.rows = this.getRows();
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
        let colsCount = this.maxStage * 2 * 2;
        for (let i = 0; i <= colsCount; i++) {
            cols.push(i);
        }
        return cols;
    }

    private getRows(): number[] {
        let rows = [];
        for (let i = 0; i < this.rounds.filter(r => r.stage === this.maxStage).length - 1; i++) {
            rows.push(i);
        }
        return rows;
    }

    private getRound(col, row) {
        if (col % 2 === 0 && row % 2 === 0) {
            return this.rounds[1];
        }
    }

    displayData(col, row) {
        let maxCol = this.columns.length - 1;
        
        
        if (col % 2 == 0) {
            let round: RoundModel;
            if (row % 2 == 0) {
                round = this.rounds[1];
                return this.getRoundTemplate(round);
            } else {
                return `<div class="ui-g-12 table-block ui-g-nopad"></div>`;
            }

        } else {
            return `<div class="ui-g-1 inline-block" style=""></div>`
        }

    }

    getRoundTemplate(round: RoundModel) {
        return `<div class="bracket ui-g-1 ui-g-nopad">
            <div class="bracket-participant-plate ui-g-12">
            ${round.roundId}
            </div>
            <div class="bracket-participant-plate ui-g-12">
            ${round.nextRoundId}
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