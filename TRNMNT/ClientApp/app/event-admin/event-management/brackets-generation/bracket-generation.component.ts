﻿import { Component, Input } from '@angular/core';
import '../../../shared/styles/brackets.scss';
import './bracket-generation.component.scss';
import { ViewEncapsulation } from '@angular/core';
import { BracketService } from '../../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../../core/model/category-with-division-filter.model';
import { RoundModel } from '../../../core/model/round.models';
import { BracketModel } from '../../../core/model/bracket.models';
import Participantmodels = require('../../../core/model/participant.models');
import ParticipantModelBase = Participantmodels.ParticipantModelBase;

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
    dragModel: DragModel;
    isEdited: boolean = false;
    maxStage: number;
    stages: number[] = [];

    private filter: CategoryWithDivisionFilterModel = new CategoryWithDivisionFilterModel('', '');

    constructor(private bracketService: BracketService) {
    }

    ngOnInit() {
        this.bracketService.createBracket('55C3F11D-EA09-4628-A49B-209AA35C4DB5').subscribe(r => {
            this.bracket = r;
            this.maxStage = this.getMaxStage(this.bracket.roundModels.length);
            this.rounds =
                this.bracket.roundModels.filter(r => r.stage == this.maxStage);
            this.initStages(this.maxStage);
        });
    }

    private createBracket() {
        this.bracketService.createBracket(this.filter.weightDivisionId).subscribe(r => {
            this.bracket = r;
            this.maxStage = this.getMaxStage(this.bracket.roundModels.length);
            this.rounds =
                this.bracket.roundModels.filter(r => r.stage == this.maxStage);
            this.initStages(this.maxStage);
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

    private getRoundIndex(col, row): number {
        if (col == 0) {
            return row / 2;
        } else {
            return this.rounds.length / 2 + row / 2;
        }
    }

    private displayParticipantInfo(round: RoundModel, participantNumber: number) {
        let participant;
        if (participantNumber == 1) {
            participant = round.firstParticipant;
        } else if (participantNumber == 2) {
            participant = round.secondParticipant;
        }
        if (participant == undefined) {
            return '';
        } else {
            return participant.firstName + ' ' + participant.lastName;
        }


    }


    private dragStart(roundIndex: number, participantNumber: number) {
        this.dragMode = true;
        this.dragModel = new DragModel(roundIndex, participantNumber);

    }

    private dragEnd() {
        this.dragMode = false;
    }

    private dragEnter($event) {
        $event.target.style.backgroundColor = '#b5e8b1';
    }

    private dragLeave($event) {
        $event.target.style.backgroundColor = 'white';
    }


    private onDrop(roundIndex: number, participantNumber: number, $event) {
        this.dragEnd();
        this.dragLeave($event);


        let targetParticipant = this.getParticipant(this.rounds[roundIndex], participantNumber);
        let sourceParticipant = this.getParticipant(this.rounds[this.dragModel.roundIndex], this.dragModel.participantNumber);
        this.setParticipant(this.rounds[this.dragModel.roundIndex], this.dragModel.participantNumber, targetParticipant);
        this.setParticipant(this.rounds[roundIndex], participantNumber, sourceParticipant);
        this.isEdited = true;
    }


    private setFilter($event) {
        this.filter = $event;
    }

    private getParticipant(round: RoundModel, pNumber: number) {
        if (pNumber == 1) {
            return round.firstParticipant;
        } else {
            return round.secondParticipant;
        }
    }

    setParticipant(round: RoundModel, pNumber: number, value: ParticipantModelBase) {
        if (pNumber == 1) {
            round.firstParticipant = value;
        } else {
            round.secondParticipant = value;
        }
    }

    private downloadBracket() {
        this.bracketService.downloadBracket(this.filter.weightDivisionId, this.getBracketsFileName()).subscribe();
    }

    private getBracketsFileName(): string {
        return 'bracket.xlsx';
    }

    private updateBracket() {
        this.bracketService.updateBracket(this.bracket).subscribe();
        this.isEdited = false;
    }

    private initStages(maxStage:number) {
        this.stages = [];
        for (var i = 0; i < 2 * maxStage; i++) {
            this.stages.push(i);
        }
    }

}

class DragModel {
    constructor(rIndex: number, pNumber: number) {
        this.participantNumber = pNumber;
        this.roundIndex = rIndex;
    }
    participantNumber: number;
    roundIndex: number;
}