import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import './event-run.component.scss'
import {BracketModel} from '../../core/model/bracket.models';
import {BracketService} from '../../core/services/bracket.service';


@Component({
    selector: 'event-run',
    templateUrl: './event-run.component.html',
})
export class EventRunComponent implements OnInit {

    eventId: string;
    bracket: BracketModel;

    constructor(private loggerService: LoggerService, private bracketService: BracketService, private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
            this.bracketService.getBracket('F31D47EC-CEE0-4A6B-B988-FA0F49FC9ED3').subscribe(r => {
                this.bracket = r;
            });
        });

    }



}
