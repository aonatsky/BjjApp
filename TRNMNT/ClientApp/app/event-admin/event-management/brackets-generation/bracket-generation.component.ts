import { Component, Input } from '@angular/core';
import '../../../shared/styles/brackets.scss';
import './bracket-generation.component.scss';
import { ViewEncapsulation } from '@angular/core';
import { BracketService } from '../../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../../core/model/category-with-division-filter.model';
import {RoundModel} from '../../../core/model/round.models';
import {BracketModel} from '../../../core/model/bracket.models';

@Component({
    selector: 'bracket-generation',
    templateUrl: 'bracket-generation.component.html',
    encapsulation: ViewEncapsulation.None
})
export class BracketGenerationComponent {
    @Input() eventId: string;
    rounds: RoundModel[] = [];
    bracket: BracketModel;
    coumnsCount: number = 3;
    dragMode: boolean = false;
    private filter: CategoryWithDivisionFilterModel = new CategoryWithDivisionFilterModel('', '');

    constructor(private bracketService: BracketService) {
    }

    ngOnInit() {

    }

    private createBracket() {
        this.bracketService.createBracket(this.filter.weightDivisionId).subscribe(r => {
            debugger;
            this.bracket = r;
            this.rounds =
                this.bracket.roundModels.filter(r => r.stage == this.getMaxStage(this.bracket.roundModels.length));
        });
    }


    private getRows() {
        var rows = [];
        for (var i = 0; i < this.rounds.length - 1; i++) {
            rows.push(i);
        }
        return rows;
    }

    private getColumns() {
        return [0, 1, 2];
    }


    private getMaxStage(roundsCount: number): number {
        for (var i = 0; i < 5; i++) {
            roundsCount -= Math.pow(2, i);
            if (roundsCount == 0) {
                return i;
            }
        }
    }

    private getRound(col, row): RoundModel {
        debugger;
        if (col == 0) {
            return this.rounds[row / 2];
        } else {
            return this.rounds[this.rounds.length / 2 + row / 2];
        }
    }

    private dragStart() {
        this.dragMode = true;
    }

    private dragEnd() {
        this.dragMode = false;
    }

    private setFilter($event) {
        this.filter = $event;
    }

}