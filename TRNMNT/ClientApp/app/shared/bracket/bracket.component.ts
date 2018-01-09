import { Component, OnInit, Input } from '@angular/core';
import { DragDropModule } from 'primeng/primeng';

;@Component({
    selector: 'bracket',
    templateUrl: './bracket.component.html',
    styleUrls: ['./bracket.component.css']
})

export class BracketComponent {
    public user: any;

    constructor() {
        this.user = {
            name: "First Name Last Name",
            score: 1
        }
    }

    dragStart(even: any, value: any) {
        debugger;
        console.log("drag started");
    }

    dragEnd(even: any) {
        debugger;
        console.log("drag ended");
    }

    drop(even: any) {
        debugger;
        console.log("drop");
    }
}