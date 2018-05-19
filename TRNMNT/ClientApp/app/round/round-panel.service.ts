import { HubConnection, TransportType} from "@aspnet/signalr";
import { RoundDetailsModel } from "../core/model/round-details/round-details.model";
import { Observable } from 'rxjs/Observable';

export abstract class BaseRoundPanel {
    private readonly hubMethodName: string = "Send";

    private hubConnection: HubConnection;
    private reconnectionCounter: number = 0;
    private readonly reconnectionLimit: number = 10;


    setupConnection(groupId: any, messageHandler: (data: RoundDetailsModel) => void): void {
        this.hubConnection = new HubConnection('/round-hub');
        this.hubConnection.start().then(() => {
            this.subscribeOnRecieveMessage(messageHandler);
            this.hubConnection.invoke("JoinGroup", groupId);
            this.bindReconnection();
        });

    }

    subscribeOnRecieveMessage(messageHandler: (data: RoundDetailsModel) => void): void {
        this.hubConnection.on(this.hubMethodName, messageHandler);
    }

    sendHubMessage(data: RoundDetailsModel): void {
        this.hubConnection.invoke(this.hubMethodName, data);
    }

    onConnectionClosed(): Observable<string> {
        return Observable.fromEventPattern(
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
            let hubHttpConnection = this.hubConnection["connection"];
            hubHttpConnection.connectionState = 0;
            let url = hubHttpConnection.url;
            hubHttpConnection.url = url
                .replace(`?id=${hubHttpConnection.connectionId}`, "")
                .replace(`&id=${hubHttpConnection.connectionId}`, "");
            this.reconnectionCounter++;
            this.hubConnection.start();
        });
    }
}