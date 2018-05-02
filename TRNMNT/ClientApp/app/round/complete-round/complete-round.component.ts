import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import { RoundResultModel } from '../../core/model/round-result.model';
import { RoundResultType } from '../../core/enums/round-result-type.enum';
import { SubmissionType } from '../../core/enums/submission-type.enum';

@Component({
    selector: 'complete-round',
    templateUrl: './complete-round.component.html',
})
export class CompleteRoundComponent implements OnInit {
    @Input() roundDetails: RoundDetailsModel;
    @Output() onClose = new EventEmitter<any>();

    private roundResultModel: RoundResultModel; 
    private submissionTypes = SubmissionType;
    private roundResultTypes = RoundResultType;


    
    private submissionType: number;

    ngOnInit(): void {
        this.initRoundResultModel();
    }

    initRoundResultModel() {
        this.roundResultModel = new RoundResultModel();
        this.roundResultModel.roundId = this.roundDetails.roundId;
        this.roundResultModel.firstParticipantPoints = this.roundDetails.firstParticipantPoints;
        this.roundResultModel.firstParticipantAdvantages = this.roundDetails.firstParticipantAdvantages;
        this.roundResultModel.firstParticipantPenalties = this.roundDetails.firstParticipantPenalties;

        this.roundResultModel.secondParticipantPoints = this.roundDetails.secondParticipantPoints;
        this.roundResultModel.secondParticipantAdvantages = this.roundDetails.secondParticipantAdvantages;
        this.roundResultModel.secondParticipantPenalties = this.roundDetails.secondParticipantPenalties;

        this.setWinnerByPoints();
    }

    private setWinnerByPoints() {
        this.roundResultModel.roundResultType = this.roundResultTypes.Points;
        if (this.roundDetails.firstParticipantPoints !== this.roundDetails.secondParticipantPoints) {
            this.roundResultModel.winnerParticipantId = this.roundDetails.firstParticipantPoints > this.roundDetails.secondParticipantPoints
                ? this.roundDetails.roundModel.firstParticipant.participantId
                : this.roundDetails.roundModel.secondParticipant.participantId;
        }
        else if (this.roundDetails.firstParticipantPoints !== this.roundDetails.secondParticipantPoints) {
            this.roundResultModel.winnerParticipantId = this.roundDetails.firstParticipantAdvantages > this.roundDetails.secondParticipantAdvantages
                ? this.roundDetails.roundModel.firstParticipant.participantId
                : this.roundDetails.roundModel.secondParticipant.participantId;
        }
        else if (this.roundDetails.firstParticipantPenalties !== this.roundDetails.secondParticipantPenalties) {
            this.roundResultModel.winnerParticipantId = this.roundDetails.firstParticipantPenalties < this.roundDetails.secondParticipantPenalties
                ? this.roundDetails.roundModel.firstParticipant.participantId
                : this.roundDetails.roundModel.secondParticipant.participantId;
        }
    }

    save(): void {
        console.log(this.roundResultModel);
      
    }

    close() {
        this.onClose.emit(null);
    }
}