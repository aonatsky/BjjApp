import { HubConnection, TransportType} from "@aspnet/signalr";
import { RoundDetailsModel } from "../core/model/round-details/round-details.model";
import {SignalRHubService} from '../core/dal/signalr/signalr-hub.service';

export abstract class RoundPanelBase {
    private readonly hubMethodName: string = "Send";

    
    private hubConnection: HubConnection;
//    private signalRService: SignalRHubService;
    isConnected: boolean = false;
    
    constructor(private signalRService: SignalRHubService) {
        this.signalRService = signalRService;
        this.hubConnection = this.signalRService.createConnection('/round-hub');
    }

    setupConnection(groupId: any, messageHandler: (data: RoundDetailsModel) => void): void {
        if (this.isConnected) {
            this.signalRService.joinGroup(groupId);
        } else {
            this.signalRService.onConnected().subscribe(() => {
                this.signalRService.joinGroup(groupId);
                this.subscribeOnRecieveMessage(messageHandler);
            });
            this.signalRService.onDisconnected().subscribe(() => this.signalRService.leaveGroup(groupId));
            this.connect();
        }
        
//        this.hubConnection = new HubConnection('/round-hub');
//        this.hubConnection.start().then(() => {
//            this.subscribeOnRecieveMessage(messageHandler);
//            this.hubConnection.invoke("JoinGroup", groupId);
//        });
    }

    subscribeOnRecieveMessage(messageHandler: (data: RoundDetailsModel) => void): void {
        this.hubConnection.on(this.hubMethodName, messageHandler);
    }

    sendHubMessage(data: RoundDetailsModel): void {
        this.hubConnection.invoke(this.hubMethodName, data);
    }

    connect(): Promise<void>{
        if (this.isConnected) {
            return new Promise((resolve, reject) => reject());
        }
        this.signalRService.bindReconnection();
        const promise = this.signalRService.start();
        this.isConnected = true;
        return promise;

    }

    disconnect() {
        console.log('DISCONNECTED');
        this.signalRService.unbindReconnection();
        const promise = this.signalRService.stop();
        this.isConnected = false;
        return promise;
    }
}