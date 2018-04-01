import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RoundModel } from '../../core/model/round.models';
import { SignalRHubService } from '../../core/dal/signalr/signalr-hub.service';
import { RoundDetailsModel } from '../../core/model/round-details/round-details.model';
import { HubConnection } from "@aspnet/signalr-client";
import './round-panel.component.scss';

@Component({
    selector: 'round-panel',
    templateUrl: './round-panel.component.html',
})
export class RoundPanelComponent implements OnInit {
    @Input() roundModel: RoundModel;

    private roundDetails: RoundDetailsModel;

    private readonly tick: number;
    private timerSubscription: any;

    private hubConnection: HubConnection;

    constructor(private signalRService: SignalRHubService) {
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
    }

    ngOnInit() {
        this.roundDetails.roundId = this.roundModel.roundId;
        this.setupConnection();
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

    public getFirstPlayerScore(): number {
        return this.roundDetails.firstPlayerAdvantage + this.roundDetails.firstPlayerPoints - this.roundDetails.firstPlayerPenalty;
    }

    public getSecondPlayerScore(): number {
        return this.roundDetails.secondPlayerAdvantage + this.roundDetails.secondPlayerPoints - this.roundDetails.secondPlayerPenalty;
    }

    public subscribeOnRecieveMessage(): void {
        this.hubConnection.on("Send", x => {
            this.roundDetails = x;

            if (this.roundDetails.isStarted) {
                this.startTimer();
            }
            if (this.roundDetails.isPaused || this.roundDetails.isCompleted) {
                this.stopTimer();
            }
        });
    }

    public send(): void {
        this.hubConnection.invoke("Send", this.roundDetails);
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

    private setupConnection(): void {
        this.hubConnection = new HubConnection('/round-hub');
        this.hubConnection.start().then(() => {
            this.subscribeOnRecieveMessage();
            this.hubConnection.invoke("JoinGroup", this.roundModel.roundId);
        });
    }
}
