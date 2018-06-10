import { Component, Input } from '@angular/core';
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

    private createBracket() {
        this.bracket = undefined;
        this.bracketService.getBracket(this.filter.weightDivisionId).subscribe(r => {
            this.bracket = r;
            this.maxStage = this.getMaxStage(this.bracket.roundModels.filter(r => r.roundType != 1).length);
            this.rounds =
                this.bracket.roundModels.filter(r => r.stage == this.maxStage).sort((r1, r2) => { return r1.order - r2.order });
        this.initStages(this.maxStage);
    });
}


    private getRows() {
    let rows = [];
    for (let i = 0; i < this.rounds.length - 1; i++) {
        rows.push(i);
    }
    return rows;
}

    private getColumns() {
    return [0, 1, 2];
}


    private getMaxStage(roundsCount: number): number {
    for (let i = 0; i < 5; i++) {
        roundsCount -= Math.pow(2, i);
        if (roundsCount === 0) {
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

    private getRound(col, row): RoundModel {
    return this.rounds[this.getRoundIndex(col, row)];

}

    private displayParticipantInfo(round: RoundModel, participantNumber: number) {
    let participant;
    if (participantNumber == 1) {
        participant = round.AParticipant;
    } else if (participantNumber == 2) {
        if (round.roundType) {
            return 'Lost in previous round';
        }
        participant = round.secondParticipant;
    }
    if (participant == undefined) {
        return '';
    } else {
        return `${participant.firstName} ${participant.lastName} (${participant.teamName})`;
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
        return round.AParticipant;
    } else {
        return round.secondParticipant;
    }
}

    private getDraggable(round: RoundModel, num: number): string {
    if (round.roundType == 2 && num == 2) {
        return '';
    }
    else {
        return 'participnatPlate';
    }
}

setParticipant(round: RoundModel, pNumber: number, value: ParticipantModelBase) {
    if (pNumber == 1) {
        round.AParticipant = value;
    } else {
        round.secondParticipant = value;
    }
}

    private downloadBracket() {
    this.bracketService.downloadBracket(this.filter.weightDivisionId, this.getBracketsFileName()).subscribe();
}

    private getBracketsFileName(): string {
    const name = this.bracket.title.replace('/', '-');
    return `${name}.xlsx`;
}

    private updateBracket() {
    this.bracketService.updateBracket(this.bracket).subscribe();
    this.isEdited = false;
}

    private initStages(maxStage: number) {
    this.stages = [];
    for (let i = 0; i < 2 * maxStage; i++) {
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