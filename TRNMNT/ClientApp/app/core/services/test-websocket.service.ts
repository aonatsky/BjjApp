import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Rx';
import { WebsocketService } from '../dal/websocket/websocket.service';

export interface Message {
    message: string
}
let WebsocketUrl = "ws://" + window.location.host + "/ws";
//let WebsocketUrl = "ws://echo.websocket.org/";

@Injectable()
export class TestWebsocketService {
    public messages: Subject<Message>;

    constructor(wsService: WebsocketService) {
        this.messages = <Subject<Message>>wsService
            .connect(WebsocketUrl)
            .map((response: MessageEvent): Message => {
                let data = JSON.parse(response.data);
                return {
                    message: data.message
                }
            });
    }

}