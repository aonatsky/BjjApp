import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BracketModel } from '../../core/model/bracket.models';
// import './../../event-admin/event-management/brackets-generation/bracket-generation.component.scss';
import { ViewEncapsulation } from '@angular/core';
import { MatchModel } from '../../core/model/match.models';

@Component({
  selector: 'bracket',
  templateUrl: './bracket.component.html',
  styleUrls:['./bracket.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BracketComponent implements OnInit {
  @Input() bracket: BracketModel;
  @Input() fullView: boolean = true;
  @Input() adminView: boolean = false;
  @Output() roundClick: EventEmitter<MatchModel> = new EventEmitter();
  maxRound: number = 0;
  columns: number[];

  constructor() {}

  ngOnInit() {
    this.maxRound = this.getMaxRound();
    this.columns = this.getColumns();
  }

  private getMaxRound(): number {
    let matchesCount = this.bracket.matchModels.filter(r => r.matchType !== 1).length;

    for (let i = 0; i < 5; i++) {
      matchesCount -= Math.pow(2, i);
      if (matchesCount === 0) {
        return i;
      }
    }
  }

  private getColumns(): number[] {
    const cols = [];
    const maxColumn = this.maxRound * 2;
    for (let i = 0; i <= maxColumn; i++) {
      cols.push(i);
    }
    return cols;
  }

  private addEmptyMatch(colNumber: number): boolean {
    if (colNumber == (this.columns.length - 1) / 2) {
      return this.bracket.matchModels.length > 1;
    }
    return false;
  }

  private getMatchSideClass(colNumber): string {
    return colNumber > (this.columns.length - 1) / 2 ? 'right-side' : '';
  }

  private getMatchTypeClass(matchModel: MatchModel): string {
    let matchTypeClass = '';
    if (matchModel.round == 0) {
      matchTypeClass = matchModel.matchType == 1 ? 'third-place' : 'final';
      if (this.bracket.matchModels.length === 1) {
        matchTypeClass += ' only-final';
      }
    }
    return matchTypeClass;
  }

  private getRoundModels(colNumber: number): MatchModel[] {
    const maxCol = this.columns.length - 1;
    const isRightSide = colNumber > maxCol / 2;
    const depth = isRightSide ? maxCol - colNumber : colNumber;
    const round = this.maxRound - depth;
    const models = this.bracket.matchModels.filter(m => m.round == round).sort((m1, m2) => {
      return m1.order - m2.order;
    });
    if (round === 0) {
      return models;
    } else {
      if (isRightSide) {
        return models.splice(models.length / 2, models.length);
      } else {
        return models.splice(0, models.length / 2);
      }
    }
  }

  private displayParticipantData(model: MatchModel, participantNumber: number) {
    if (participantNumber == 1) {
      return model.aParticipant ? model.aParticipant.firstName + ' ' + model.aParticipant.lastName : '';
    } else {
      if (model.bParticipant) {
        return model.bParticipant.firstName + ' ' + model.bParticipant.lastName;
      } else if (model.matchType == 2) {
        return 'LOST FIRST ROUND';
      } else {
        return '';
      }
    }
  }

  private displayMatchResult(model: MatchModel, participantNumber: number) {
    if (participantNumber === 1) {
      return model.aParticipantResult;
    }
  }

  private onMatchClick(model: MatchModel) {
    if (this.isEditable(model)) {
      this.roundClick.emit(model);
    }
  }

  private getWinnerClass(model: MatchModel) {
    if (model.winnerParticipant) {
      if (!!model.aParticipant && model.winnerParticipant.participantId === model.aParticipant.participantId) {
        return 'winner-1';
      } else {
        return 'winner-2';
      }
    } else {
      return '';
    }
  }

  private isEditable(model: MatchModel): boolean {
    if(!this.adminView){
      return false;
    }
    if (model.aParticipant && model.bParticipant) {
      if (model.winnerParticipant && model.nextMatchId) {
        const nextRound = this.bracket.matchModels.filter(r => r.matchId === model.nextMatchId)[0];
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

  getFullViewClass(colNumber: number){
    if(!this.fullView && colNumber != this.maxRound * 2 && colNumber != 0){
      return 'collapsed';
    }
    return ''
  }
}
