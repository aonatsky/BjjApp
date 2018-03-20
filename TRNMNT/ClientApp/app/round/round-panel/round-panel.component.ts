import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RoundModel } from '../../core/model/round.models';
import { SignalRHubService } from '../../core/dal/signalr/signalr-hub.service';
import { HubConnection } from "@aspnet/signalr-client";
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import './round-panel.component.scss';

@Component({
    selector: 'round-panel',
    templateUrl: './round-panel.component.html',
})
export class RoundPanelComponent implements OnInit {
    @Input() roundModel: RoundModel;

    private hubConnection: HubConnection;
    private roundDetails: RoundDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;

    constructor(private signalRService: SignalRHubService) {
        this.roundDetails = new RoundDetailsModel();

        this.roundDetails.firstPlayerPenalty = 0;
        this.roundDetails.secondPlayerPenalty = 0;
        this.roundDetails.firstPlayerAdvantage = 0;
        this.roundDetails.secondPlayerAdvantage = 0;

        this.roundDetails.countdown = 2 * 60;
        this.roundDetails.isStarted = false;
        this.roundDetails.isPaused = false;
        this.roundDetails.isCompleted = false;

        this.tick = 1000;
    }

    ngOnInit() {
        this.hubConnection = new HubConnection("/round-hub");
        this.subscribeOnRecieveMessage();
        this.hubConnection.start().then(() => console.log("connected"), x=>console.log(x));
    }

    public start(): void {
        this.startTimer();
        this.roundDetails.isStarted = true;
        this.roundDetails.isPaused = false;
        this.roundDetails.isCompleted = false;
        this.send();
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
        this.send();
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

    public subscribeOnRecieveMessage(): void {
        this.hubConnection.on("Send", x => {
            this.roundDetails = JSON.parse(x);

            if (this.roundDetails.isStarted) {
                this.startTimer();
            }
            if (this.roundDetails.isPaused || this.roundDetails.isCompleted) {
                this.stopTimer();
            }
        });
    }

    public send(): void {
        this.hubConnection.invoke("Send", JSON.stringify(this.roundDetails));
    }

    private startTimer(): void {
        if (!this.timerSubscription) {
            this.timerSubscription = Observable.timer(0, this.tick).subscribe(() => --this.roundDetails.countdown);
        }
    }

    private stopTimer(): void {
        if (this.timerSubscription) {
            this.timerSubscription.unsubscribe();
        }
        this.timerSubscription = null;
    }
}
