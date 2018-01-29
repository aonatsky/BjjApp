import { HubConnection, TransportType } from '@aspnet/signalr-client';
import { LoggerService } from '../../services/logger.service';
import { Injectable } from '@angular/core';


@Injectable()
export class SignalRHubService {
    constructor(private loggerService: LoggerService) {
        
    }

    public connect(url: string, isReconnect: boolean = false, transport?: TransportType): HubConnection {
        let options = transport != null ? { transport: transport } : null;
        let hubConnection = new HubConnection(url, options);
        let connection: any = hubConnection;
        hubConnection.start()
            .then(() => {
                this.loggerService.logDebug(`Connection started: id: ${connection.id}`);
            })
            .catch(err => {
                this.loggerService.logDebug(`Error while establishing connection ${err}`);
            });
        hubConnection.onclose((exc) => {
            this.loggerService.logDebug(`Connection closed ${exc}`);
            if (isReconnect) {
                setTimeout(() => hubConnection.start(), 1000);
            }
        });
        return hubConnection;
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