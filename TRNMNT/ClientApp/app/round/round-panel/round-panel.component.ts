import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable, timer } from 'rxjs';
import { BaseRoundPanel } from '../round-panel.service';
import './round-panel.component.scss';
import { MatchModel } from '../../core/model/match.models';
import { MatchDetailsModel } from '../../core/model/match-details.model';

@Component({
    selector: 'round-panel',
    templateUrl: './round-panel.component.html',
})
export class RoundPanelComponent extends BaseRoundPanel implements OnInit {
    @Input() matchModel: MatchModel;
    @Output() completeRound: EventEmitter<any> = new EventEmitter();
    @Output() close: EventEmitter<any> = new EventEmitter();

    matchDetails: MatchDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;
    private displayPopup: boolean;

    constructor() {
        super();
        this.matchDetails = new MatchDetailsModel();

        this.matchDetails.aParticipantPenalties = 0;
        this.matchDetails.bParticipantPenalties = 0;
        this.matchDetails.aParticipantAdvantages = 0;
        this.matchDetails.bParticipantAdvantages = 0;
        this.matchDetails.aParticipantPoints = 0;
        this.matchDetails.bParticipantPoints = 0;

        this.matchDetails.countdown = 2 * 60;
        this.matchDetails.isStarted = false;
        this.matchDetails.isPaused = false;
        this.matchDetails.isCompleted = false;

        this.tick = 1000;

        this.displayPopup = false;
    }

    ngOnInit() {
        this.matchDetails.matchModel = this.matchModel;
        this.matchDetails.matchId = this.matchModel.matchId;
        this.matchDetails.countdown = this.matchDetails.matchModel.matchTime;
        this.setupConnection(this.matchModel.matchId, x => {
            this.matchDetails = x;
            this.matchDetails.matchModel = this.matchModel;
            if (this.matchDetails.isStarted) {
                this.startTimer();
            }
            if (this.matchDetails.isPaused || this.matchDetails.isCompleted) {
                this.stopTimer();
            }
        });
    }

    start(): void {
        if (!this.matchDetails.isCompleted || !this.timerSubscription) {
            this.startTimer();
            this.matchDetails.isStarted = true;
            this.matchDetails.isPaused = false;
            this.matchDetails.isCompleted = false;
            this.send();
        }
    }

    pause(): void {
        this.stopTimer();
        this.matchDetails.isStarted = false;
        this.matchDetails.isPaused = true;
        this.matchDetails.isCompleted = false;
        this.send();
    }

    stop(): void {
        this.stopTimer();
        this.matchDetails.isStarted = false;
        this.matchDetails.isPaused = false;
        this.matchDetails.isCompleted = true;
        this.displayPopup = true;
        this.send();
    }

    resetTimer(): void {
        this.matchDetails.countdown = this.matchDetails.matchModel.matchTime;
        this.stopTimer();
        this.matchDetails.isStarted = false;
        this.matchDetails.isPaused = false;
        this.matchDetails.isCompleted = true;
        this.send();
    }

    changeAParticipantAdvantages(advantagesStep: number): void {
        this.matchDetails.aParticipantAdvantages = this.matchDetails.aParticipantAdvantages + advantagesStep < 0 ? 0 : this.matchDetails.aParticipantAdvantages + advantagesStep;
        this.send();
    }

    changeBParticipantAdvantages(advantagesStep: number): void {
        this.matchDetails.bParticipantAdvantages = this.matchDetails.bParticipantAdvantages + advantagesStep < 0 ? 0 : this.matchDetails.bParticipantAdvantages + advantagesStep;
        this.send();
    }

    changeAParticipantPenalties(penaltiesStep: number): void {
        this.matchDetails.aParticipantPenalties = this.matchDetails.aParticipantPenalties + penaltiesStep < 0 ? 0 : this.matchDetails.aParticipantPenalties + penaltiesStep;
        this.send();
    }

    changeBParticipantPenalties(penaltiesStep: number): void {
        this.matchDetails.bParticipantPenalties = this.matchDetails.bParticipantPenalties + penaltiesStep < 0 ? 0 : this.matchDetails.bParticipantPenalties + penaltiesStep;
        this.send();
    }

    changeAParticipantPoints(pointStep: number): void {
        this.matchDetails.aParticipantPoints = this.matchDetails.aParticipantPoints + pointStep < 0 ? 0 : this.matchDetails.aParticipantPoints + pointStep;
        this.send();
    }

    changeBParticipantPoints(pointStep: number): void {
        this.matchDetails.bParticipantPoints = this.matchDetails.bParticipantPoints + pointStep < 0 ? 0 : this.matchDetails.bParticipantPoints + pointStep;
        this.send();
    }

    send(): void {
        this.sendHubMessage(this.matchDetails);
    }

    private startTimer(): void {
        if (!this.timerSubscription) {
            this.timerSubscription = timer(0, this.tick).subscribe(() => {
                --this.matchDetails.countdown;
                if (this.matchDetails.countdown <= 0) {
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
