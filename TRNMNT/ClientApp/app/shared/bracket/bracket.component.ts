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
    roundGroups: RoundModel[][];
    maxStage: number = 0;
    rows: number[];
    columns: number[];

    constructor() {

    }

    ngOnInit() {

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
        if (col % 2 == 0) {

            let roundStage = this.maxStage - depth;

            if (depth === this.maxStage) {
                depth = depth - 1;
                roundStage = 0;
            }

            let shift = (Math.pow(2, depth) - 1);
            let freq = Math.pow(2, 1 + depth);
            if ((row - shift) % freq == 0) {
                let roundIndex = (isRightSide ? this.roundGroups[roundStage].length / 2 : 0) + ((row - shift) / freq);;
                let round: RoundModel = this.roundGroups[roundStage][roundIndex];
                return this.getRoundTemplate(round);
            } else {
                return `<div class="ui-g-12 table-block ui-g-nopad"></div>`;
            }

        } else {
            return this.getConnectorTemplate(row, depth - 0.5, isRightSide);
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

    getConnectorTemplate(row: number, depth: number, isRightSide: boolean) {
        let startShift = (Math.pow(2, depth) - 1);
        let endShift = ((Math.pow(2, depth) - 1) + Math.pow(2, depth + 1));
        let centerShift = (startShift + endShift) / 2;
        let freq = Math.pow(2, 2 + depth);
        let style: ConnectorStyle = new ConnectorStyle();

        if (depth == this.maxStage - 1) {
            if ((row - startShift) % freq == 0) {
                 style = this.horLine();
            }
        } else {
            if ((row - startShift) % freq == 0) {
                style = this.lowCorner(isRightSide);
            }
            if ((row - endShift) % freq == 0) {
                style = this.highCorner(isRightSide);
            }
            for (let j = 0; j < centerShift; j++) {
                if ((row - Math.pow(2, depth) - j) % freq == 0) {
                    style = this.vertLine(isRightSide);
                }
            }
            if ((row - centerShift) % freq == 0) {
                style = this.tConnector(isRightSide);
            }
        }




        return `<div class="connector"><div class="ui-g-6 ui-g-nopad ${style.topLeftClass}"></div>
                <div class="ui-g-6 ui-g-nopad ${style.topRightClass}"></div>
                <div class="ui-g-6 ui-g-nopad ${style.bottomLeftClass}"></div>
                <div class="ui-g-6 ui-g-nopad ${style.bottomRightClass}"></div></div>`;
    }

    private lowCorner(isRightSide: boolean): ConnectorStyle {
        return isRightSide
            ? new ConnectorStyle('', 'border-bottom', '', 'border-left')
            : new ConnectorStyle('border-bottom', '', 'border-right', '');

    }

    private highCorner(isRightSide: boolean) {
        return isRightSide
            ? new ConnectorStyle('', 'border-bottom border-left', '', '')
            : new ConnectorStyle('border-bottom border-right', '', '', '');
    }

    private vertLine(isRightSide: boolean) {
        return isRightSide
            ? new ConnectorStyle('', 'border-left', '', 'border-left')
            : new ConnectorStyle('border-right', '', 'border-right', '');
    }

    private tConnector(isRightSide: boolean) {
        return isRightSide
            ? new ConnectorStyle('border-bottom ', 'border-left', '', 'border-left')
            : new ConnectorStyle('border-right', 'border-bottom', 'border-right', '');
    }


    private horLine() {
        return new ConnectorStyle('border-bottom ', 'border-bottom', '', '');

    }

    getColumnClass(col) {
        if (col % 2 == 0) {
            return 'brackets-column';
        } else {
            return 'connector-column';
        }

    }
}

class ConnectorStyle {
    constructor(public topLeftClass: string = '', public topRightClass: string = '', public bottomLeftClass: string = '', public bottomRightClass: string = '') { }

}