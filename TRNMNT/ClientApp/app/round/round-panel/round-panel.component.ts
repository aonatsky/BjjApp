import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RoundModel} from '../../core/model/round.models';
import './round-panel.component.scss';

@Component({
    selector: 'round-panel',
    templateUrl: './round-panel.component.html',
})
export class RoundPanelComponent implements OnInit {

    @Input() roundModel: RoundModel;
    
    private readonly tick: number;
    private readonly penaltyStep: number;
    private readonly advantageStep: number;
    private countdown: number;
    private timerSubscription: any;
    private isRoundCompleted: boolean;

    private firstPlayerPenalty: number;
    private secondPlayerPenalty: number;
    private firstPlayerAdvantage: number;
    private secondPlayerAdvantage: number;

    constructor() {
        this.firstPlayerPenalty = 0;
        this.secondPlayerPenalty = 0;
        this.firstPlayerAdvantage = 0;
        this.secondPlayerAdvantage = 0;

        this.countdown = 2 * 60;
        this.tick = 1000;
        this.penaltyStep = 1;
        this.advantageStep = 2;

        this.isRoundCompleted = false;
    }

    ngOnInit() {

    }

    public startTimer(): void {
        this.timerSubscription = Observable.timer(0, this.tick)
            .subscribe(() => --this.countdown);
    }

    public pauseTimer(): void {
        this.timerSubscription.unsubscribe();
        this.timerSubscription = null;
    }

    public stopTimer(): void {
        if (this.timerSubscription) {
            this.timerSubscription.unsubscribe();
        }
        this.timerSubscription = null;
        this.isRoundCompleted = true;
    }

    public increaseFirstPlayerAdvantage(): void {
        this.firstPlayerAdvantage = this.firstPlayerAdvantage + this.advantageStep;
    }

    public decreaseFirstPlayerAdvantage(): void {
        this.firstPlayerAdvantage = this.firstPlayerAdvantage - this.advantageStep < 0 ? 0 : this.firstPlayerAdvantage - this.advantageStep;
    }

    public increaseSecondPlayerAdvantage(): void {
        this.secondPlayerAdvantage = this.secondPlayerAdvantage + this.advantageStep;
    }

    public decreaseSecondPlayerAdvantage(): void {
        this.secondPlayerAdvantage = this.secondPlayerAdvantage - this.advantageStep < 0 ? 0 : this.secondPlayerAdvantage - this.advantageStep;
    }

    public increaseFirstPlayerPenalty(): void {
        this.firstPlayerPenalty = this.firstPlayerPenalty + this.penaltyStep;
    }

    public decreaseFirstPlayerPenalty(): void {
        this.firstPlayerPenalty = this.firstPlayerPenalty - this.penaltyStep < 0 ? 0 : this.firstPlayerPenalty - this.penaltyStep;
    }

    public increaseSecondPlayerPenalty(): void {
        this.secondPlayerPenalty = this.secondPlayerPenalty + this.penaltyStep;
    }

    public decreaseSecondPlayerPenalty(): void {
        this.secondPlayerPenalty = this.secondPlayerPenalty - this.penaltyStep < 0 ? 0 : this.secondPlayerPenalty - this.penaltyStep;
    }
}
