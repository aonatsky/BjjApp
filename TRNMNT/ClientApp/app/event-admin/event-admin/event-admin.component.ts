import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { AuthService } from './../../core/services/auth.service';
import { ParticipantModelBase } from '../../core/model/participant.models';
import './event-admin.component.scss'
import { MatchModel } from '../../core/model/match.models';


@Component({
    selector: 'event-admin',
    templateUrl: './event-admin.component.html',
})
export class EventAdminComponent implements OnInit {

    userData: string;
    data: MatchModel;

    constructor(private loggerService: LoggerService, private authService: AuthService) {
        this.data = new MatchModel();
        this.data.matchId = 'asdfasdf';
        this.data.nextMatchId = 'asdf';
        this.data.round = 1;
        this.data.aParticipant = new ParticipantModelBase();
        this.data.aParticipant.participantId = 'partId0';
        this.data.aParticipant.firstName = '0FU';
        this.data.aParticipant.lastName = '0LU';
        this.data.aParticipant.dateOfBirth = new Date('12/10/2000');

        this.data.bParticipant = new ParticipantModelBase();
        this.data.bParticipant.participantId = 'partId1';
        this.data.bParticipant.firstName = '1FU';
        this.data.bParticipant.lastName = '1LU';
        this.data.bParticipant.dateOfBirth = new Date('10/10/2000');
    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }
}
