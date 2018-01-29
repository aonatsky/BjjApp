import { HubConnection, TransportType } from '@aspnet/signalr-client';
import { LoggerService } from '../../services/logger.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SignalRHubService {

    private hubConnection: HubConnection;
    private logConnectionDelegate: (id: string) => void;
    private logDisconnectionDelegate: (id: string, exception: any) => void;

    constructor(private loggerService: LoggerService) {
        this.logConnectionDelegate = id => this.logConnection(id);
        this.logDisconnectionDelegate = (id, exception) => this.logDisconnection(id, exception);
    }

    public createConnection(url: string, transport?: TransportType): HubConnection {
        if (this.hubConnection) {
            return this.hubConnection;
        }
        let transportType = transport || this.detectSupportedTransport();
        this.hubConnection = new HubConnection(url, { transport: transportType });

        this.hubConnection.on('ClientDisconnected', this.logDisconnectionDelegate);
        this.hubConnection.on('ClientConnected', this.logConnectionDelegate);
        this.hubConnection.onclose((exc) => {
            this.loggerService.logDebug(`Connection closed : ${exc}`);
        });
        return this.hubConnection;
    }

    public subscribeOnEvent(eventName: string): Observable<any> {
        return Observable.fromEventPattern(
            (handler: (...args: any[]) => void) => this.hubConnection.on(eventName, handler),
            (handler: (...args: any[]) => void) => this.hubConnection.off(eventName, handler)
        );
    }

    public start(): void {
        this.hubConnection.start();
    }

    public stop(): void {
        this.hubConnection.off('ClientDisconnected', this.logDisconnectionDelegate);
        this.hubConnection.off('ClientConnected', this.logConnectionDelegate);
        this.hubConnection.stop();
    }

    private logConnection(id: string): void {
        this.loggerService.logDebug(`ClientConnected: ${id}`);
    }

    private logDisconnection(id: string, exception: any): void {
        if (!!exception) {
            this.loggerService.logError(`ClientDisconnected: ${id}, exception: ${exception}`);
        } else {
            this.loggerService.logDebug(`ClientDisconnected: ${id}`);
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