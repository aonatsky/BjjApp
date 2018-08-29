import { Component, Input, OnInit } from '@angular/core';
import './bracket-generation.component.scss';
import { ViewEncapsulation } from '@angular/core';
import { BracketService } from '../../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../../core/model/category-with-division-filter.model';
import { BracketModel } from '../../../core/model/bracket.models';
import { MatchModel } from '../../../core/model/match.models';
import { ParticipantModelBase } from '../../../core/model/participant.models';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'bracket-generation',
  templateUrl: 'bracket-generation.component.html',
  encapsulation: ViewEncapsulation.None
})
export class BracketGenerationComponent implements OnInit {
  @Input()
  eventId: string;

  matches: MatchModel[] = [];
  bracket: BracketModel;
  columnsCount: number = 3;
  dragMode: boolean = false;
  dragModel: DragModel;
  isEdited: boolean = false;
  maxRound: number;
  rounds: number[] = [];
  bracketsCreated: boolean;
  filter: CategoryWithDivisionFilterModel = new CategoryWithDivisionFilterModel('', '');

  constructor(private bracketService: BracketService, private translateService: TranslateService) {}

  ngOnInit(): void {
    this.bracketService.areBracketsCreated(this.eventId).subscribe(r => {
      this.bracketsCreated = r;
    });
  }

  public getBracket() {
    this.bracket = undefined;
    this.bracketService.getBracket(this.filter.weightDivisionId).subscribe(r => {
      this.bracket = r;
      this.maxRound = this.getMaxRound(this.bracket.matchModels.filter(m => m.matchType !== 1).length);
      this.matches = this.bracket.matchModels.filter(m => m.round === this.maxRound).sort((r1, r2) => {
        return r1.order - r2.order;
      });
      this.initRounds(this.maxRound);
    });
  }

  private getRows() {
    let rows = [];
    for (let i = 0; i < this.matches.length - 1; i++) {
      rows.push(i);
    }
    return rows;
  }

  private getColumns() {
    return [0, 1, 2];
  }

  private getMaxRound(matchesCount: number): number {
    for (let i = 0; i < 5; i++) {
      matchesCount -= Math.pow(2, i);
      if (matchesCount === 0) {
        return i;
      }
    }
  }

  private getMatchIndex(col, row): number {
    if (col == 0) {
      return row / 2;
    } else {
      return this.matches.length / 2 + row / 2;
    }
  }

  private getMatch(col, row): MatchModel {
    return this.matches[this.getMatchIndex(col, row)];
  }

  private displayParticipantInfo(match: MatchModel, participantNumber: number) {
    let participant;
    if (participantNumber == 1) {
      participant = match.aParticipant;
    } else if (participantNumber == 2) {
      if (match.matchType) {
        return this.translateService.instant('EVENT.LOST_IN_PREVIOUS_MATCH');
      }
      participant = match.bParticipant;
    }
    if (participant == undefined) {
      return '';
    } else {
      return `${participant.firstName} ${participant.lastName} (${participant.teamName})`;
    }
  }

  private dragStart(matchIndex: number, participantNumber: number) {
    this.dragMode = true;
    this.dragModel = new DragModel(matchIndex, participantNumber);
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

  private onDrop(matchIndex: number, participantNumber: number, $event) {
    this.dragEnd();
    this.dragLeave($event);

    let targetParticipant = this.getParticipant(this.matches[matchIndex], participantNumber);
    let sourceParticipant = this.getParticipant(
      this.matches[this.dragModel.matchIndex],
      this.dragModel.participantNumber
    );
    this.setParticipant(this.matches[this.dragModel.matchIndex], this.dragModel.participantNumber, targetParticipant);
    this.setParticipant(this.matches[matchIndex], participantNumber, sourceParticipant);
    this.isEdited = true;
  }

  private setFilter($event) {
    this.filter = $event;
  }

  private getParticipant(match: MatchModel, pNumber: number) {
    if (pNumber === 1) {
      return match.aParticipant;
    } else {
      return match.bParticipant;
    }
  }

  private getDraggable(match: MatchModel, num: number): string {
    if (match.matchType === 2 && num === 2) {
      return '';
    } else {
      return 'participnatPlate';
    }
  }

  setParticipant(match: MatchModel, pNumber: number, value: ParticipantModelBase) {
    if (pNumber === 1) {
      match.aParticipant = value;
    } else {
      match.bParticipant = value;
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

  private initRounds(maxRound: number) {
    this.rounds = [];
    if (maxRound == 0) {
      this.rounds.push(0);
    }
    for (let i = 0; i < 2 * maxRound; i++) {
      this.rounds.push(i);
    }
  }

  createBrackets() {
    this.bracketService.createBrackets(this.eventId).subscribe(() => (this.bracketsCreated = true));
  }

  deleteBrackets() {
    this.bracketService.deleteBrackets(this.eventId).subscribe(() => {
      this.bracketsCreated = false;
      this.bracket = undefined;
    });
  }
}

class DragModel {
  constructor(rIndex: number, pNumber: number) {
    this.participantNumber = pNumber;
    this.matchIndex = rIndex;
  }
  participantNumber: number;
  matchIndex: number;
}
