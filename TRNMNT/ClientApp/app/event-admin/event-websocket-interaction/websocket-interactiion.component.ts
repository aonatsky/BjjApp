import { Component } from '@angular/core';
import { WebsocketService } from '../../core/dal/websocket/websocket.service';
import { TestWebsocketService, Message } from '../../core/services/test-websocket.service';

@Component({
    selector: 'websocket-interactiion',
    templateUrl: './websocket-interactiion.component.html',
    providers: [WebsocketService, TestWebsocketService]
})
export class WebsocketInteractionComponent {

    constructor(private chatService: TestWebsocketService) {
        chatService.messages.subscribe(msg => {
            this.messages.push(msg);
        });
    }

    private messages: Message[] = [];
    private message = {
        message: 'this is a test message'
    }

    sendMsg() {
        console.log('new message from client to websocket: ', this.message);
        this.chatService.messages.next(this.message);
    }

}