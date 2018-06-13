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
        this.data.AParticipant = new ParticipantModelBase();
        this.data.AParticipant.participantId = "partId0";
        this.data.AParticipant.firstName = "0FU";
        this.data.AParticipant.lastName = "0LU";
        this.data.AParticipant.dateOfBirth = new Date("12/10/2000");

        this.data.BParticipant = new ParticipantModelBase();
        this.data.BParticipant.participantId = "partId1";
        this.data.BParticipant.firstName = "1FU";
        this.data.BParticipant.lastName = "1LU";
        this.data.BParticipant.dateOfBirth = new Date("10/10/2000");
    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }
}
