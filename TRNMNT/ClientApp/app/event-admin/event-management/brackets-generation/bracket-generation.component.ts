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
    
    coumnsCount: number = 3;
    bracketsCount: number = 4;
    bracket: BracketModel;


    constructor(private bracketService: BracketService) {
        this.users = [
            {
                name: 'Participant 1',
                score: 1
            },
            {
                name: 'Participant 2',
                score: 1
            },
            {
                name: 'Participant 3',
                score: 1
            },
            {
                name: 'Participant 4',
                score: 1
            },
            {
                name: 'Participant 5',
                score: 1
            },
            {
                name: 'Participant 6',
                score: 1
            },
            {
                name: 'Participant 7',
                score: 1
            },
            {
                name: 'Participant 8',
                score: 1
            }
        ];


    }

    ngOnInit() {
        this.bracketService.createBracket('83CA0F48-1BF7-4441-E54A-08D4C85DC99E').subscribe(res => {
            debugger;
            this.bracket = res;
        });
    }

    
    private getRows() {
        var rows = [];
        for (var i = 0; i < this.users.length/2 - 1; i++) {
            rows.push(i);
        }
        return rows;
    }

    private getColumns() {
        return [0, 1, 2];
    }

    private getParticipant(col, row) {
        if (col == 0) {
            return this.users[row];
        } else {
            return this.users[this.users.length / 2 + row];
        }
    }
}