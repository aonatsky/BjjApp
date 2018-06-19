import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseRoundPanel } from '../round-panel.service';
import './round-panel-view.component.scss';
import { MatchModel } from '../../core/model/match.models';
import { MatchDetailsModel } from '../../core/model/match-details.model';


@Component({
    selector: 'round-panel-view',
    templateUrl: './round-panel-view.component.html',
})
export class RoundPanelViewComponent extends BaseRoundPanel implements OnInit {
    @Input() matchModel: MatchModel;
    @Input() title: string;

    private matchDetails: MatchDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;

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
    }

    ngOnInit(): void {
        this.matchDetails.countdown = this.matchModel.matchTime;
        this.matchDetails.matchId = this.matchModel.matchId;
        this.setupConnection(this.matchModel.matchId, x => {
            this.matchDetails = x;

            if (this.matchDetails.isStarted) {
                this.startTimer();
            }
            if (this.matchDetails.isPaused || this.matchDetails.isCompleted) {
                this.stopTimer();
            }
        });
    }

    private startTimer(): void {
        if (!this.timerSubscription) {
            this.timerSubscription = Observable.timer(0, this.tick).subscribe(() => {
                --this.matchDetails.countdown;
                if (this.matchDetails.countdown <= 0) {
                    this.stopTimer();
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