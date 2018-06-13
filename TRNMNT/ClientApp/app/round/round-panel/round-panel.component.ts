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

        this.roundDetails.AParticipantPenalties = 0;
        this.roundDetails.BParticipantPenalties = 0;
        this.roundDetails.AParticipantAdvantages = 0;
        this.roundDetails.BParticipantAdvantages = 0;
        this.roundDetails.AParticipantPoints = 0;
        this.roundDetails.BParticipantPoints = 0;

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

    changeAParticipantAdvantages(advantagesStep: number): void {
        this.roundDetails.AParticipantAdvantages = this.roundDetails.AParticipantAdvantages + advantagesStep < 0 ? 0 : this.roundDetails.AParticipantAdvantages + advantagesStep;
        this.send();
    }

    changeBParticipantAdvantages(advantagesStep: number): void {
        this.roundDetails.BParticipantAdvantages = this.roundDetails.BParticipantAdvantages + advantagesStep < 0 ? 0 : this.roundDetails.BParticipantAdvantages + advantagesStep;
        this.send();
    }

    changeAParticipantPenalties(penaltiesStep: number): void {
        this.roundDetails.AParticipantPenalties = this.roundDetails.AParticipantPenalties + penaltiesStep < 0 ? 0 : this.roundDetails.AParticipantPenalties + penaltiesStep;
        this.send();
    }

    changeBParticipantPenalties(penaltiesStep: number): void {
        this.roundDetails.BParticipantPenalties = this.roundDetails.BParticipantPenalties + penaltiesStep < 0 ? 0 : this.roundDetails.BParticipantPenalties + penaltiesStep;
        this.send();
    }

    changeAParticipantPoints(pointStep: number): void {
        this.roundDetails.AParticipantPoints = this.roundDetails.AParticipantPoints + pointStep < 0 ? 0 : this.roundDetails.AParticipantPoints + pointStep;
        this.send();
    }

    changeBParticipantPoints(pointStep: number): void {
        this.roundDetails.BParticipantPoints = this.roundDetails.BParticipantPoints + pointStep < 0 ? 0 : this.roundDetails.BParticipantPoints + pointStep;
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
