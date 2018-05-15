import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { LoggerService } from './../../core/services/logger.service';
import { BracketService } from '../../core/services/bracket.service';
import { ParticipantInAbsoluteDivisionMobel as ParticipantSmallTableModel } from '../../core/model/participant.models';

@Component({
    selector: 'absolute-weight-division',
    templateUrl: './absolute-weight-division.component.html',
})
export class AbsoluteWeightDivisionComponent implements OnInit {

    @Input() categoryId: string;
    @Input() isAllWinnersSelected: boolean;
    @Output() saveAbsolute: EventEmitter<any> = new EventEmitter();

    private candidates: ParticipantSmallTableModel[];
    private selectedParticipants: ParticipantSmallTableModel[] = [];
    private sortDirection: number = 1;
    private sortField: string = 'firstName';
    private displayAbsoluteWindow: boolean = false;
    private prevCategoryId: string;

    constructor(
        private loggerService: LoggerService,
        private bracketService: BracketService) {
    }
    columnsData: any[] = [
        { propertyName: 'firstName', displayName: 'First Name', isSortable: false},
        { propertyName: 'lastName', displayName: 'Last Name', isSortable: false },
        { propertyName: 'weightDivisionName', displayName: 'Weight division', isSortable: false}
    ];

    ngOnInit() {
        debugger;
    }

    showAbsoluteWeightDivision() {
        debugger;
        if (this.prevCategoryId !== this.categoryId) {
            this.prevCategoryId = this.categoryId;
            this.bracketService.getWinnersByCategory(this.categoryId).subscribe(data => {
                this.candidates = data.filter(p => !p.isSelectedIntoDivision);
                this.selectedParticipants = data.filter(p => p.isSelectedIntoDivision);
            });
        }
        
        this.displayAbsoluteWindow = !this.displayAbsoluteWindow;
    }

    private createAbsoluteWeightDivision() {
        this.bracketService.manageAbsoluteWeightDivision(this.selectedParticipantIds, this.categoryId).subscribe(() => {
            this.displayAbsoluteWindow = false;
            this.saveAbsolute.emit(null);
        });
    }

    private get selectedParticipantIds() {
        return this.selectedParticipants.map(p => p.participantId);
    }
}
