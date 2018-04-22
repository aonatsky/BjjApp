import { Component, OnInit, Input } from '@angular/core';
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

    private roundDetails: RoundDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;
    private displayPopup: boolean;

    constructor() {
        super();
        this.roundDetails = new RoundDetailsModel();

        this.roundDetails.firstPlayerPenalty = 0;
        this.roundDetails.secondPlayerPenalty = 0;
        this.roundDetails.firstPlayerAdvantage = 0;
        this.roundDetails.secondPlayerAdvantage = 0;
        this.roundDetails.firstPlayerPoints = 0;
        this.roundDetails.secondPlayerPoints = 0;

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
        this.setupConnection(this.roundModel.roundId, x => {
            this.roundDetails = x;

            if (this.roundDetails.isStarted) {
                this.startTimer();
            }
            if (this.roundDetails.isPaused || this.roundDetails.isCompleted) {
                this.stopTimer();
            }
        });
    }

    public start(): void {
        if (!this.roundDetails.isCompleted || !this.timerSubscription) {
            this.startTimer();
            this.roundDetails.isStarted = true;
            this.roundDetails.isPaused = false;
            this.roundDetails.isCompleted = false;
            this.send();
        }
    }

    public pause(): void {
        this.stopTimer();
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = true;
        this.roundDetails.isCompleted = false;
        this.send();
    }

    public stop(): void {
        this.stopTimer();
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = false;
        this.roundDetails.isCompleted = true;
        this.displayPopup = true;
        this.send();
    }

    public resetTimer(): void {
        this.roundDetails.countdown = 2 * 60;
        this.stop();
    }

    public changeFirstPlayerAdvantage(advantageStep: number): void {
        this.roundDetails.firstPlayerAdvantage = this.roundDetails.firstPlayerAdvantage + advantageStep < 0 ? 0 : this.roundDetails.firstPlayerAdvantage + advantageStep;
        this.send();
    }

    public changeSecondPlayerAdvantage(advantageStep: number): void {
        this.roundDetails.secondPlayerAdvantage = this.roundDetails.secondPlayerAdvantage + advantageStep < 0 ? 0 : this.roundDetails.secondPlayerAdvantage + advantageStep;
        this.send();
    }

    public changeFirstPlayerPenalty(penaltyStep: number): void {
        this.roundDetails.firstPlayerPenalty = this.roundDetails.firstPlayerPenalty + penaltyStep < 0 ? 0 : this.roundDetails.firstPlayerPenalty + penaltyStep;
        this.send();
    }

    public changeSecondPlayerPenalty(penaltyStep: number): void {
        this.roundDetails.secondPlayerPenalty = this.roundDetails.secondPlayerPenalty + penaltyStep < 0 ? 0 : this.roundDetails.secondPlayerPenalty + penaltyStep;
        this.send();
    }

    public changeFirstPlayerPoints(pointStep: number): void {
        this.roundDetails.firstPlayerPoints = this.roundDetails.firstPlayerPoints + pointStep < 0 ? 0 : this.roundDetails.firstPlayerPoints + pointStep;
        this.send();
    }

    public changeSecondPlayerPoints(pointStep: number): void {
        this.roundDetails.secondPlayerPoints = this.roundDetails.secondPlayerPoints + pointStep < 0 ? 0 : this.roundDetails.secondPlayerPoints + pointStep;
        this.send();
    }

    public send(): void {
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
}
