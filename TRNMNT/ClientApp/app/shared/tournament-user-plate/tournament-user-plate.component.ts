import { Component, Input } from '@angular/core';

; @Component({
    selector: 'tournament-user-plate',
    templateUrl: './tournament-user-plate.component.html',
    styleUrls: ['./tournament-user-plate.component.css']
})

export class TournamentUserPlate {
    @Input() user: any;
}