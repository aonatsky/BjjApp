import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RoundResultType } from '../../core/enums/round-result-type.enum';
import { SubmissionType } from '../../core/enums/submission-type.enum';
import { BracketService } from '../../core/services/bracket.service';
import { MatchResultModel } from '../../core/model/match-result.model';
import { MatchDetailsModel } from '../../core/model/match-details.model';


@Component({
    selector: 'complete-round',
    templateUrl: './complete-round.component.html',
})
export class CompleteRoundComponent implements OnInit {
    @Input() matchDetails: MatchDetailsModel;
    @Output() close = new EventEmitter<any>();
    @Output() complete = new EventEmitter<any>();


    private matchResultModel: MatchResultModel;
    private submissionTypes = SubmissionType;
    private roundResultTypes = RoundResultType;

    constructor(private bracketService: BracketService) { }

    ngOnInit(): void {
        this.initRoundResultModel();
    }

    initRoundResultModel() {
        this.matchResultModel = new MatchResultModel();
        this.matchResultModel.roundId = this.matchDetails.matchId;
        this.matchResultModel.aParticipantPoints = this.matchDetails.aParticipantPoints;
        this.matchResultModel.aParticipantAdvantages = this.matchDetails.aParticipantAdvantages;
        this.matchResultModel.aParticipantPenalties = this.matchDetails.aParticipantPenalties;

        this.matchResultModel.bParticipantPoints = this.matchDetails.bParticipantPoints;
        this.matchResultModel.bParticipantAdvantages = this.matchDetails.bParticipantAdvantages;
        this.matchResultModel.bParticipantPenalties = this.matchDetails.bParticipantPenalties;
        this.matchResultModel.completeTime = this.matchDetails.matchModel.matchTime - this.matchDetails.countdown;
        this.setWinnerByPoints();
    }

    private setWinnerByPoints() {

        if (this.matchDetails.aParticipantPoints !== this.matchDetails.bParticipantPoints) {
            this.matchResultModel.winnerParticipantId = this.matchDetails.aParticipantPoints > this.matchDetails.bParticipantPoints
                ? this.matchDetails.matchModel.aParticipant.participantId
                : this.matchDetails.matchModel.bParticipant.participantId;
            this.matchResultModel.roundResultType = this.roundResultTypes.Points;
        }
        else if (this.matchDetails.aParticipantAdvantages !== this.matchDetails.bParticipantAdvantages) {
            this.matchResultModel.winnerParticipantId = this.matchDetails.aParticipantAdvantages > this.matchDetails.bParticipantAdvantages
                ? this.matchDetails.matchModel.aParticipant.participantId
                : this.matchDetails.matchModel.bParticipant.participantId;
            this.matchResultModel.roundResultType = RoundResultType.Advantages;
        }
        else if (this.matchDetails.aParticipantPenalties !== this.matchDetails.bParticipantPenalties) {
            this.matchResultModel.winnerParticipantId = this.matchDetails.aParticipantPenalties <
                this.matchDetails.bParticipantPenalties
                ? this.matchDetails.matchModel.aParticipant.participantId
                : this.matchDetails.matchModel.bParticipant.participantId;
            this.matchResultModel.roundResultType = RoundResultType.Penalties;
        } else {
            this.matchResultModel.winnerParticipantId = this.matchDetails.matchModel.aParticipant.participantId;
            this.matchResultModel.roundResultType = RoundResultType.Decision;
        }

    }

    save(): void {
        this.bracketService.setRoundResult(this.matchResultModel).subscribe(r => {
            this.complete.emit(null);
        });
    }

    isDisabled(): boolean {
        return this.matchResultModel.roundResultType === this.roundResultTypes.Submission;
    }
}