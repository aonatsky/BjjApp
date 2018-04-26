import { HubConnection, TransportType } from '@aspnet/signalr';
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

    createConnection(url: string, transport?: TransportType): HubConnection {
        if (this.hubConnection) {
            return this.hubConnection;
        }
        let transportType = transport || this.detectSupportedTransport();
        this.hubConnection = new HubConnection(url, { transport: transportType });

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

    start(): void {
        this.hubConnection.start();
    }

    stop(): void {
        this.hubConnection.off('OtherClientDisconnected', this.logDisconnectionDelegate);
        this.hubConnection.off('OtherClientConnected', this.logConnectionDelegate);
        this.hubConnection.stop();
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

    joinGroup(groupName: string) {
        this.hubConnection.invoke("JoinGroup", groupName);
    }

    leaveGroup(groupName: string) {
        this.hubConnection.invoke("LeaveGroup", groupName);
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

    private detectSupportedTransport(): TransportType {
        let $window: any = window;
        if (!!$window.WebSocket) {
            return TransportType.WebSockets;
        }
        if (!!$window.EventSource) {
            return TransportType.ServerSentEvents;
        }
        return TransportType.LongPolling;
    }

}