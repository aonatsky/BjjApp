import { Component } from '@angular/core';
import { SignalRHubService } from '../../core/dal/signalr/signalr-hub.service';
import { HubConnection, TransportType } from '@aspnet/signalr-client';

@Component({
    selector: 'websocket-interaction',
    templateUrl: './websocket-interaction.component.html',
    providers: [SignalRHubService]
})
export class WebsocketInteractionComponent {

    private hubConnection: HubConnection;

    constructor(private signalRService: SignalRHubService, private signalRService2: SignalRHubService) {
    }

    ngOnInit() {
        this.hubConnection = this.signalRService.connect("/chat", true, TransportType.LongPolling);
        this.hubConnection.on('SendAsync',
            (data: any) => {
                const received = `Received: ${data}`;
                this.messages.push({
                    message: received
                });
            });
        //let hubConnection = this.signalRService2.connect("/chat", true, TransportType.LongPolling);
        this.hubConnection.on('ClientDisconnectedAsync',
            (id: any, exception: any) => {
                const received = `ClientDisconnectedAsync: ${id}, exception: ${exception}`;
                this.messages.push({
                    message: received
                });
            });
        this.hubConnection.on('ClientDisconnectedAsync',
            (id: any) => {
                const received = `ClientConnectedAsync: ${id}`;
                this.messages.push({
                    message: received
                });
            });
    }

    private messages: any[] = [];
    private message = {
        message: 'this is a test message'
    }

    sendMsg() {
        const data = `Sent: ${this.message.message}`;
        this.hubConnection.invoke('Send', data);
    }

}