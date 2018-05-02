import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RoundModel } from '../../core/model/round.models';
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import { BaseRoundPanel } from '../round-panel.service';
import './round-panel-view.component.scss';


@Component({
    selector: 'round-panel-view',
    templateUrl: './round-panel-view.component.html',
})
export class RoundPanelViewComponent extends BaseRoundPanel implements OnInit {
    @Input() roundModel: RoundModel;

    private roundDetails: RoundDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;

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
    }

    ngOnInit(): void {
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

    private startTimer(): void {
        if (!this.timerSubscription) {
            this.timerSubscription = Observable.timer(0, this.tick).subscribe(() => {
                --this.roundDetails.countdown;
                if (this.roundDetails.countdown <= 0) {
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