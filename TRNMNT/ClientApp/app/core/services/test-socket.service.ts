import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { Observable } from 'rxjs/Rx';
import { SignalRHubService } from "../dal/signalr/signalr-hub.service";
import { TransportType, HubConnection } from "@aspnet/signalr-client";


@Injectable()
export class TestSocketService {
    private hubConnection: HubConnection;

    constructor(private loggerService: LoggerService, private signalRService: SignalRHubService) {
        this.hubConnection = this.signalRService.createConnection("/chat", TransportType.LongPolling);
    }

    public recieveMessage(): Observable<string> {
        return this.signalRService.subscribeOnEvent("Send");
    }

    public send(message: string): void {
        this.hubConnection.invoke("Send", message);
    }

    public connect() {
        this.signalRService.start();
    }

    public disconnect() {
        this.signalRService.stop();
    }
}