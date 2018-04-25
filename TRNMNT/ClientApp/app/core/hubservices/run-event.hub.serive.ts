import { Injectable } from "@angular/core"
import { Observable } from 'rxjs/Rx';
import { SignalRHubService } from "../dal/signalr/signalr-hub.service";
import { TransportType, HubConnection } from "@aspnet/signalr";
import { RefreshBracketModel } from "../model/bracket.models";


@Injectable()
export class RunEventHubService {
    private hubConnection: HubConnection;

    isConnected: boolean = false;

    constructor(private signalRService: SignalRHubService) {
        this.hubConnection = this.signalRService.createConnection("/runevent", TransportType.ServerSentEvents);
    }

    joinWeightDivisionGroup(weightDivisionId: string, previousWeightDivisionId?: string) {
        let id = weightDivisionId.toUpperCase();
        if (this.isConnected) {
            if (!!previousWeightDivisionId) {
                this.signalRService.leaveGroup(previousWeightDivisionId.toLowerCase());
            }
            this.signalRService.joinGroup(id);
        } else {
            this.signalRService.onConnected().subscribe(() => this.signalRService.joinGroup(id));
            this.signalRService.onDisconnected().subscribe(() => this.signalRService.leaveGroup(id));
            this.connect();
        }
    }

    joinMultipleWeightDivisionGroups(weightDivisionIds: string[]) {
        if (!this.isConnected) {
            this.signalRService.onConnected().subscribe(() => this.joinGroups(weightDivisionIds));
            this.signalRService.onDisconnected().subscribe(() => this.leaveGroups(weightDivisionIds));
            this.connect();
        }
    }

    joinGroups(groupNames: string[]) {
        for (var groupName of groupNames) {
            let id = groupName.toUpperCase();
            this.signalRService.joinGroup(id);
        }
    }

    leaveGroups(groupNames: string[]) {
        for (var groupName of groupNames) {
            let id = groupName.toUpperCase();
            this.signalRService.leaveGroup(id);
        }
    }

    onRefreshRound(): Observable<RefreshBracketModel> {
        return this.signalRService.subscribeOnEvent("BracketRoundsUpdated");
    }

    connect() {
        if (this.isConnected) {
            return;
        }
        this.signalRService.bindReconnection();
        this.signalRService.start();
        this.isConnected = true;
    }

    disconnect() {
        this.signalRService.stop();
        this.isConnected = false;
    }
}