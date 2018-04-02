import { Component, OnInit, Input } from '@angular/core';
import { LoggerService } from './../../core/services/logger.service';
import { BracketService } from '../../core/services/bracket.service';
import './absolute-weight-division.component.scss'
import { ParticipantSmallTableModel } from '../../core/model/participant.models';

@Component({
    selector: 'absolute-weight-division',
    templateUrl: './absolute-weight-division.component.html',
})
export class AbsoluteWeightDivisionComponent implements OnInit {

    @Input() categoryId: string;
    private candidates: ParticipantSmallTableModel[];
    private selectedParticipants: ParticipantSmallTableModel[];
    private sortDirection: number = 1;
    private sortField: string = "firstName";
    private displayAbsoluteWindow: boolean = false;

    constructor(
        private loggerService: LoggerService,
        private bracketService: BracketService) {
    }
    columnsData: any[] = [
        { propertyName: "firstName", displayName: "First Name", isSortable: true},
        { propertyName: "lastName", displayName: "Last Name", isSortable: true },
        { propertyName: "teamName", displayName: "Team", isSortable: true},
        { propertyName: "weightDivisionName", displayName: "Weight division", isSortable: true}
    ];

    ngOnInit() {
        this.bracketService.getWinnersByCategory(this.categoryId).subscribe(data => this.candidates = data);
    }

    public showAbsoluteWeightDivision() {
        this.displayAbsoluteWindow = !this.displayAbsoluteWindow;
    }

    private createAbsoluteWeightDivision() {
    }
}
