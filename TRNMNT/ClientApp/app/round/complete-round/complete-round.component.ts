import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import { RoundResult } from '../../core/enums/round-result.enum';
import { SubmissionType } from '../../core/enums/submission-type.enum';
import './complete-round.component.scss';

@Component({
    selector: 'complete-round',
    templateUrl: './complete-round.component.html',
})
export class CompleteRoundComponent implements OnInit {
    @Input() roundDetails: RoundDetailsModel;
    @Output() onClose = new EventEmitter<any>();

    private submissionTypes = SubmissionType;
    private roundResults = RoundResult;

    private winnerId: string;
    private roundResult: number;
    private submissionType: number;
    
    public ngOnInit(): void {
        this.winnerId = this.roundDetails.roundModel.firstParticipant.participantId;
        this.roundResult = this.roundResults.Points;
        this.submissionType = this.submissionTypes.Armlock;
    }

    public save(): void {
        console.log(this.winnerId);
        console.log(this.roundResult);
        console.log(this.submissionType);
    }

    public close() {
        this.onClose.emit(null);
    }
}