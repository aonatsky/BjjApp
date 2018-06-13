import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import { RoundResultModel } from '../../core/model/round-result.model';
import { RoundResultType } from '../../core/enums/round-result-type.enum';
import { SubmissionType } from '../../core/enums/submission-type.enum';
import { BracketService } from '../../core/services/bracket.service';


@Component({
    selector: 'complete-round',
    templateUrl: './complete-round.component.html',
})
export class CompleteRoundComponent implements OnInit {
    @Input() roundDetails: RoundDetailsModel;
    @Output() close = new EventEmitter<any>();
    @Output() complete = new EventEmitter<any>();


    private roundResultModel: RoundResultModel;
    private submissionTypes = SubmissionType;
    private roundResultTypes = RoundResultType;

    constructor(private bracketService: BracketService) { }

    ngOnInit(): void {
        this.initRoundResultModel();
    }

    initRoundResultModel() {
        this.roundResultModel = new RoundResultModel();
        this.roundResultModel.roundId = this.roundDetails.roundId;
        this.roundResultModel.AParticipantPoints = this.roundDetails.AParticipantPoints;
        this.roundResultModel.AParticipantAdvantages = this.roundDetails.AParticipantAdvantages;
        this.roundResultModel.AParticipantPenalties = this.roundDetails.AParticipantPenalties;

        this.roundResultModel.BParticipantPoints = this.roundDetails.BParticipantPoints;
        this.roundResultModel.BParticipantAdvantages = this.roundDetails.BParticipantAdvantages;
        this.roundResultModel.BParticipantPenalties = this.roundDetails.BParticipantPenalties;
        this.roundResultModel.completeTime = this.roundDetails.roundModel.roundTime - this.roundDetails.countdown;
        this.setWinnerByPoints();
    }

    private setWinnerByPoints() {

        if (this.roundDetails.AParticipantPoints !== this.roundDetails.BParticipantPoints) {
            this.roundResultModel.winnerParticipantId = this.roundDetails.AParticipantPoints > this.roundDetails.BParticipantPoints
                ? this.roundDetails.roundModel.AParticipant.participantId
                : this.roundDetails.roundModel.BParticipant.participantId;
            this.roundResultModel.roundResultType = this.roundResultTypes.Points;
        }
        else if (this.roundDetails.AParticipantAdvantages !== this.roundDetails.BParticipantAdvantages) {
            this.roundResultModel.winnerParticipantId = this.roundDetails.AParticipantAdvantages > this.roundDetails.BParticipantAdvantages
                ? this.roundDetails.roundModel.AParticipant.participantId
                : this.roundDetails.roundModel.BParticipant.participantId;
            this.roundResultModel.roundResultType = RoundResultType.Advantages;
        }
        else if (this.roundDetails.AParticipantPenalties !== this.roundDetails.BParticipantPenalties) {
            this.roundResultModel.winnerParticipantId = this.roundDetails.AParticipantPenalties <
                this.roundDetails.BParticipantPenalties
                ? this.roundDetails.roundModel.AParticipant.participantId
                : this.roundDetails.roundModel.BParticipant.participantId;
            this.roundResultModel.roundResultType = RoundResultType.Penalties;
        } else {
            this.roundResultModel.winnerParticipantId = this.roundDetails.roundModel.AParticipant.participantId;
            this.roundResultModel.roundResultType = RoundResultType.Decision;
        }

    }

    save(): void {
        this.bracketService.setRoundResult(this.roundResultModel).subscribe(r => {
            this.complete.emit(null);
        });
    }

    isDisabled(): boolean {
        return this.roundResultModel.roundResultType === this.roundResultTypes.Submission;
    }
 }