import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import { LoggerService } from '../../services/logger.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SignalRHubService {

    private hubConnection: HubConnection;
    private readonly reconnectionLimit: number = 10;
    private reconnectionCounter: number = 0;
    private logConnectionDelegate: (id: string) => void;
    private logDisconnectionDelegate: (id: string, exception: any) => void;

    constructor(private loggerService: LoggerService) {
        this.logConnectionDelegate = id => this.logConnection(id);
        this.logDisconnectionDelegate = (id, exception) => this.logDisconnection(id, exception);
    }

    createConnection(url: string, transport?: HttpTransportType): HubConnection {
        if (this.hubConnection) {
            return this.hubConnection;
        }
        let transportType = transport || this.detectSupportedTransport();
        this.hubConnection = new HubConnectionBuilder().withUrl(url, transportType).build();

        this.hubConnection.on('OtherClientDisconnected', this.logDisconnectionDelegate);
        this.hubConnection.on('OtherClientConnected', this.logConnectionDelegate);
        this.hubConnection.onclose((exc) => {
            this.loggerService.logDebug(`Connection closed : ${exc}`);
        });
        return this.hubConnection;
    }

    subscribeOnEvent(eventName: string): Observable<any> {
        return Observable.fromEventPattern(
            (handler: (...args: any[]) => void) => this.hubConnection.on(eventName, handler),
            (handler: (...args: any[]) => void) => this.hubConnection.off(eventName, handler)
        );
    }

    fireEvent(eventName: string, data?: any): void {
        this.hubConnection.invoke(eventName, data);
    }

    start(): Promise<void> {
        return this.hubConnection.start();
    }

    stop(): Promise<void> {
        this.hubConnection.off('OtherClientDisconnected', this.logDisconnectionDelegate);
        this.hubConnection.off('OtherClientConnected', this.logConnectionDelegate);
        return this.hubConnection.stop();
    }

    onConnected(): Observable<string> {
        return this.subscribeOnEvent('Connected');
    }

    onDisconnected(): Observable<string> {
        return this.subscribeOnEvent('Disconnected');
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

    joinGroup(groupName: string) {
        console.log('join group: ', groupName);
        this.hubConnection.invoke('JoinGroup', groupName);
    }

    leaveGroup(groupName: string) {
        this.hubConnection.invoke('LeaveGroup', groupName);
    }

    private logConnection(connectionId: string): void {
        this.loggerService.logDebug(`ClientConnected: ${connectionId}`);
    }

    private logDisconnection(connectionId: string, exception: any): void {
        if (!!exception) {
            this.loggerService.logError(`ClientDisconnected: ${connectionId}, exception: ${exception}`);
        } else {
            this.loggerService.logDebug(`ClientDisconnected: ${connectionId}`);
        }
    }

    private detectSupportedTransport(): HttpTransportType {
        let $window: any = window;
        if (!!$window.WebSocket) {
            return HttpTransportType.WebSockets;
        }
        if (!!$window.EventSource) {
            return HttpTransportType.ServerSentEvents;
        }
        return HttpTransportType.LongPolling;
    }

}