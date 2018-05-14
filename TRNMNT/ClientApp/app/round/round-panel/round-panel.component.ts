import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RoundModel } from '../../core/model/round.models';
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import { BaseRoundPanel } from '../round-panel.service';
import './round-panel.component.scss';

@Component({
    selector: 'round-panel',
    templateUrl: './round-panel.component.html',
})
export class RoundPanelComponent extends BaseRoundPanel implements OnInit {
    @Input() roundModel: RoundModel;
    @Output() completeRound: EventEmitter<any> = new EventEmitter();
    @Output() close: EventEmitter<any> = new EventEmitter();

    private roundDetails: RoundDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;
    private displayPopup: boolean;

    constructor() {
        super();
        this.roundDetails = new RoundDetailsModel();

        this.roundDetails.firstParticipantPenalties = 0;
        this.roundDetails.secondParticipantPenalties = 0;
        this.roundDetails.firstParticipantAdvantages = 0;
        this.roundDetails.secondParticipantAdvantages = 0;
        this.roundDetails.firstParticipantPoints = 0;
        this.roundDetails.secondParticipantPoints = 0;

        this.roundDetails.countdown = 2 * 60;
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = false;
        this.roundDetails.isCompleted = false;

        this.tick = 1000;

        this.displayPopup = false;
    }

    ngOnInit() {
        this.roundDetails.roundModel = this.roundModel;
        this.roundDetails.roundId = this.roundModel.roundId;
        this.roundDetails.countdown = this.roundDetails.roundModel.roundTime;
        this.setupConnection(this.roundModel.roundId, x => {
            this.roundDetails = x;
            this.roundDetails.roundModel = this.roundModel;
            if (this.roundDetails.isStarted) {
                this.startTimer();
            }
            if (this.roundDetails.isPaused || this.roundDetails.isCompleted) {
                this.stopTimer();
            }
        });
    }

    start(): void {
        if (!this.roundDetails.isCompleted || !this.timerSubscription) {
            this.startTimer();
            this.roundDetails.isStarted = true;
            this.roundDetails.isPaused = false;
            this.roundDetails.isCompleted = false;
            this.send();
        }
    }

    pause(): void {
        this.stopTimer();
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = true;
        this.roundDetails.isCompleted = false;
        this.send();
    }

    stop(): void {
        this.stopTimer();
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = false;
        this.roundDetails.isCompleted = true;
        this.displayPopup = true;
        this.send();
    }

    resetTimer(): void {
        this.roundDetails.countdown = this.roundDetails.roundModel.roundTime;
        this.stopTimer();
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = false;
        this.roundDetails.isCompleted = true;
        this.send();
    }

    changeFirstParticipantAdvantages(advantagesStep: number): void {
        this.roundDetails.firstParticipantAdvantages = this.roundDetails.firstParticipantAdvantages + advantagesStep < 0 ? 0 : this.roundDetails.firstParticipantAdvantages + advantagesStep;
        this.send();
    }

    changeSecondParticipantAdvantages(advantagesStep: number): void {
        this.roundDetails.secondParticipantAdvantages = this.roundDetails.secondParticipantAdvantages + advantagesStep < 0 ? 0 : this.roundDetails.secondParticipantAdvantages + advantagesStep;
        this.send();
    }

    changeFirstParticipantPenalties(penaltiesStep: number): void {
        this.roundDetails.firstParticipantPenalties = this.roundDetails.firstParticipantPenalties + penaltiesStep < 0 ? 0 : this.roundDetails.firstParticipantPenalties + penaltiesStep;
        this.send();
    }

    changeSecondParticipantPenalties(penaltiesStep: number): void {
        this.roundDetails.secondParticipantPenalties = this.roundDetails.secondParticipantPenalties + penaltiesStep < 0 ? 0 : this.roundDetails.secondParticipantPenalties + penaltiesStep;
        this.send();
    }

    changeFirstParticipantPoints(pointStep: number): void {
        this.roundDetails.firstParticipantPoints = this.roundDetails.firstParticipantPoints + pointStep < 0 ? 0 : this.roundDetails.firstParticipantPoints + pointStep;
        this.send();
    }

    changeSecondParticipantPoints(pointStep: number): void {
        this.roundDetails.secondParticipantPoints = this.roundDetails.secondParticipantPoints + pointStep < 0 ? 0 : this.roundDetails.secondParticipantPoints + pointStep;
        this.send();
    }

    send(): void {
        this.sendHubMessage(this.roundDetails);
    }

    private startTimer(): void {
        if (!this.timerSubscription) {
            this.timerSubscription = Observable.timer(0, this.tick).subscribe(() => {
                --this.roundDetails.countdown;
                if (this.roundDetails.countdown <= 0) {
                    this.stop();
                }
            });
        }
    }

    private stopTimer(): void {
        if (this.timerSubscription) {
            this.timerSubscription.unsubscribe();
        }
        this.timerSubscription = null;
    }

    private onComplete(): void {
        this.completeRound.emit(null);
    }

    private onClose(): void {
        this.close.emit(null);
    }
}
