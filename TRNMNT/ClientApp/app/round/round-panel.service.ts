import { HubConnection } from "@aspnet/signalr";
import { RoundDetailsModel } from "../core/model/round-details/round-details.model";

export abstract class BaseRoundPanel {
    private readonly hubMethodName: string = "Send";

    private hubConnection: HubConnection;

    public setupConnection(groupId: any, messageHandler: (data: RoundDetailsModel) => void): void {
        this.hubConnection = new HubConnection('/round-hub');
        this.hubConnection.start().then(() => {
            this.subscribeOnRecieveMessage(messageHandler);
            this.hubConnection.invoke("JoinGroup", groupId);
        });
    }

    public subscribeOnRecieveMessage(messageHandler: (data: RoundDetailsModel) => void): void {
        this.hubConnection.on(this.hubMethodName, messageHandler);
    }

    public sendHubMessage(data: RoundDetailsModel): void {
        this.hubConnection.invoke(this.hubMethodName, data);
    }
}