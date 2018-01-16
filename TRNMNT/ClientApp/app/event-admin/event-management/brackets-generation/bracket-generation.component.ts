import { Component, Input } from '@angular/core';
import '../../../shared/styles/brackets.scss';
import './bracket-generation.component.scss';
import { ViewEncapsulation } from '@angular/core';
import {BracketService} from '../../../core/services/bracket.service';

@Component({
    selector: 'bracket-generation',
    templateUrl: 'bracket-generation.component.html',
    encapsulation: ViewEncapsulation.None
})
export class BracketGenerationComponent {
    @Input() users: any[];
    @Input() eventId: string;
    rounds : RoundModel[];
    
    coumnsCount: number = 3;
    bracket: BracketModel;
    dragMode: boolean = false;


    constructor(private bracketService: BracketService) {
    }

    ngOnInit() {
        this.bracketService.createBracket('83CA0F48-1BF7-4441-E54A-08D4C85DC99E').subscribe(res => {
            this.rounds = res.roundModels.filter(r => r.stage == this.getMaxStage(res.roundModels.length));
            console.log(this.rounds.length);
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


    private getMaxStage(roundsCount: number) : number {
        for (var i = 0; i < 5; i++) {
            roundsCount -= Math.pow(2, i);
            if (roundsCount == 0) {
                return i;
            }
        }
    }

    private getRound(col, row): RoundModel {
        if (col == 0) {
            return this.rounds[row/2];
        } else {
            return this.rounds[this.rounds.length / 2 + row/2];
        }
    }

    private dragStart() {
        this.dragMode = true;
    }

    private dragEnd() {
        this.dragMode = false;
    }

}