import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { Observable } from 'rxjs/Rx';
import { SignalRHubService } from "../dal/signalr/signalr-hub.service";
import { TransportType, HubConnection } from "@aspnet/signalr";


@Injectable()
export class TestSocketService {
    private hubConnection: HubConnection;

    constructor(private loggerService: LoggerService, private signalRService: SignalRHubService) {
        this.hubConnection = this.signalRService.createConnection("/chat");
    }

    recieveMessage(): Observable<string> {
        return this.signalRService.subscribeOnEvent("Send");
    }

    send(message: string): void {
        this.hubConnection.invoke("Send", message);
    }

    connect() {
        this.signalRService.start();
    }

    disconnect() {
        this.signalRService.stop();
    }
}