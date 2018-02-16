import { Injectable } from "@angular/core"
import { Observable } from 'rxjs/Rx';
import { SignalRHubService } from "../dal/signalr/signalr-hub.service";
import { TransportType, HubConnection } from "@aspnet/signalr-client";
import { BracketModel } from "../model/bracket.models";


@Injectable()
export class RunEventHubService {
    private hubConnection: HubConnection;

    public isConnected: boolean = false;

    constructor(private signalRService: SignalRHubService) {
        this.hubConnection = this.signalRService.createConnection("/runevent", TransportType.LongPolling);
    }

    public joinWeightDivisionGroup(weightDivisionId: string, previousWeightDivisionId?: string) {
        if (this.isConnected) {
            if (!!previousWeightDivisionId) {
                this.signalRService.leaveGroup(previousWeightDivisionId);
            }
            this.signalRService.joinGroup(weightDivisionId);
        } else {
            this.signalRService.onConnected().subscribe(() => this.signalRService.joinGroup(weightDivisionId));
            this.signalRService.onDisconnected().subscribe(() => this.signalRService.leaveGroup(weightDivisionId));
            this.connect();
        }
    }

    public onRefreshRound(): Observable<BracketModel> {
        return this.signalRService.subscribeOnEvent("BracketRoundsUpdated");
    }

    public connect() {
        if (this.isConnected) {
            return;
        }
        this.signalRService.start();
        this.isConnected = true;
    }

    public disconnect() {
        this.signalRService.stop();
        this.isConnected = false;
    }
}