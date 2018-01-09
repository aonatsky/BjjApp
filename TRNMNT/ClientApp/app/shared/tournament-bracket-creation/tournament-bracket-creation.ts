import { Component, OnInit, Input } from '@angular/core';
import { DragDropModule } from 'primeng/primeng';

;@Component({
    selector: 'tournament-bracket-creation',
    templateUrl: './tournament-bracket-creation.component.html',
    styleUrls: ['./tournament-bracket-creation.component.css']
})

export class TournamentBracketCreation {
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