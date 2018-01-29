import { Component, OnDestroy } from '@angular/core';
import { TestSocketService } from '../../core/services/test-socket.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'websocket-interaction',
    templateUrl: './websocket-interaction.component.html',
})
export class WebsocketInteractionComponent implements OnDestroy {
    private messageSubscription: Subscription;

    constructor(private testSocketService: TestSocketService) {
    }

    ngOnInit() {
        this.messageSubscription = this.testSocketService.recieveMessage().subscribe((data: any) => {
                const received = `Received: ${data}`;
                this.messages.push({
                    message: received
                });
            });
        this.testSocketService.connect();
    }

    private messages: any[] = [];
    private message = {
        message: 'this is a test message'
    }

    sendMsg() {
        this.testSocketService.send(`Sent: ${this.message.message}`);
    }

    ngOnDestroy(): void {
        this.messageSubscription.unsubscribe();
        this.testSocketService.disconnect();
    }

}