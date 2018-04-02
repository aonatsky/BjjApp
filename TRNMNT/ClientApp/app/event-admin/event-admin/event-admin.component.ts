import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { AuthService } from './../../core/services/auth.service';
import { RoundModel } from '../../core/model/round.models';
import { ParticipantModelBase } from '../../core/model/participant.models';
import './event-admin.component.scss'


@Component({
    selector: 'event-admin',
    templateUrl: './event-admin.component.html',
})
export class EventAdminComponent implements OnInit {

    userData: string;
    data: RoundModel;

    constructor(private loggerService: LoggerService, private authService: AuthService) {
        this.data = new RoundModel();
        this.data.roundId = "asdfasdf";
        this.data.nextRoundId = "asdf";
        this.data.stage = 1;
        this.data.firstParticipant = new ParticipantModelBase();
        this.data.firstParticipant.participantId = "0";
        this.data.firstParticipant.firstName = "0FU";
        this.data.firstParticipant.lastName = "0LU";
        this.data.firstParticipant.dateOfBirth = new Date("12/10/2000");

        this.data.secondParticipant = new ParticipantModelBase();
        this.data.secondParticipant.participantId = "1";
        this.data.secondParticipant.firstName = "1FU";
        this.data.secondParticipant.lastName = "1LU";
        this.data.secondParticipant.dateOfBirth = new Date("10/10/2000");
    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }
}
