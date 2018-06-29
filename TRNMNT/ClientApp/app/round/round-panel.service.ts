import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Observable } from 'rxjs';
import { MatchDetailsModel } from '../core/model/match-details.model';
import { fromEventPattern } from 'rxjs';

export abstract class BaseRoundPanel {
    private readonly hubMethodName: string = 'Send';

    private hubConnection: HubConnection;
    private reconnectionCounter: number = 0;
    private readonly reconnectionLimit: number = 10;


    setupConnection(groupId: any, messageHandler: (data: MatchDetailsModel) => void): void {
        this.hubConnection = new HubConnectionBuilder().withUrl('/round-hub').build();
        this.hubConnection.start().then(() => {
            this.subscribeOnRecieveMessage(messageHandler);
            this.hubConnection.invoke('JoinGroup', groupId);
            this.bindReconnection();
        });

    }

    subscribeOnRecieveMessage(messageHandler: (data: MatchDetailsModel) => void): void {
        this.hubConnection.on(this.hubMethodName, messageHandler);
    }

    sendHubMessage(data: MatchDetailsModel): void {
        this.hubConnection.invoke(this.hubMethodName, data);
    }

    onConnectionClosed(): Observable<string> {
        return fromEventPattern(
            (handler: (e: Error) => void) => this.hubConnection.onclose(handler)
        );
    }

    bindReconnection() {
        this.onConnectionClosed().subscribe((e) => {
            if (this.reconnectionCounter >= this.reconnectionLimit) {
                this.reconnectionCounter = 0;
                return;
            }
            // huck to start reconnection
            let hubHttpConnection = this.hubConnection['connection'];
            hubHttpConnection.connectionState = 0;
            let url = hubHttpConnection.url;
            hubHttpConnection.url = url
                .replace(`?id=${hubHttpConnection.connectionId}`, '')
                .replace(`&id=${hubHttpConnection.connectionId}`, '');
            this.reconnectionCounter++;
            this.hubConnection.start();
        });
    }
}