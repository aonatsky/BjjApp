import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { BracketService } from '../../core/services/bracket.service';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { RouterService } from '../../core/services/router.service';
import './absolute-weight-division.component.scss'

@Component({
    selector: 'absolute-weight-division',
    templateUrl: './absolute-weight-division.component.html',
})
export class AbsoluteWeightDivisionComponent implements OnInit {

    private candidates: any[];
    private selectedParticipants: any[];
    private sortDirection: number = 1;
    private sortDirectionBack: number = 0;
    private sortField: string = "firstName";
    private selectedSourceEntity: any;
    private selectedTargetEntity: any;
    private emptyProto: any;
    private index: number = 0;

    private get originalEntityCount(): number {
        return this.candidates.length;
    }

    private get emptyEntity(): any {
        let clone: any = Object.assign({}, this.emptyProto);
        clone.index = this.index++;
        return clone;
    }

    constructor(
        private loggerService: LoggerService,
        private bracketService: BracketService,
        private route: ActivatedRoute,
        private routerService: RouterService,
        private runEventHubService: RunEventHubService) {
    }
    columnsData: any[] = [
        { propertyName: "firstName", displayName: "First Name", isSortable: true},
        { propertyName: "lastName", displayName: "Last Name", isSortable: true },
        { propertyName: "teamName", displayName: "Team", isSortable: true},
        { propertyName: "weightDivisionName", displayName: "Weight division", isSortable: true}
    ];

    ngOnInit() {
        this.candidates = [
            {
                firstName: "Mark",
                lastName: "Voldberg",
                teamName: "teamm",
                weightDivisionName: "50"
            }, {
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                , id: 'qwer1'
            },{
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                ,id: 'qwer'
            },{
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                , id: 'qwer2'
            },{
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                , id: 'qwer3'
            },{
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                , id: 'qwer4'
            },{
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                , id: 'qwer5'
            },{
                firstName: "James",
                lastName: "Brinjer",
                teamName: "cravlers",
                weightDivisionName: "100"
                , id: 'qwer6'
            }, {
                firstName: "Tom",
                lastName: "Clorton",
                teamName: "brongs",
                weightDivisionName: "90"

            }
        ];
        this.selectedParticipants = [];
        //this.selectedSourceEntity = this.candidates[0];
        this.emptyProto = {
            firstName: "\u00a0",
            lastName: "\u00a0",
            teamName: "\u00a0",
            weightDivisionName: "\u00a0",
            index: 0,
            empty: true
        };
        this.populateArray(this.selectedParticipants);

    }

    private addItem() {
        this.pickEntity(this.selectedSourceEntity, this.candidates, this.selectedParticipants);
        if (!!this.selectedSourceEntity) {
            this.selectedTargetEntity = this.selectedSourceEntity;
            const next = this.candidates.find(p => !p.empty);
            this.selectedSourceEntity = next;
        }
    }

    private removeItem() {
        this.pickEntity(this.selectedTargetEntity, this.selectedParticipants, this.candidates);
        if (!!this.selectedTargetEntity) {
            this.selectedSourceEntity = this.selectedTargetEntity;
            const next = this.selectedParticipants.find(p => !p.empty);
            this.selectedTargetEntity = next;
        }
    }

    private populateArray<T>(array: Array<T>) {
        if (array.length < this.originalEntityCount) {
            const  len = this.originalEntityCount - array.length;
            for (let i = 0; i < len; i++) {
                array.push(this.emptyEntity);
            }
        }
    }

    private pickEntity(entity, sourceArray, targetArray) {
        if (!!entity && !entity.empty) {
            const index = sourceArray.findIndex(p => p === entity);
            const targetIndex = targetArray.findIndex(p => !!p.empty);
            targetArray.splice(targetIndex, 1, entity);
            sourceArray.splice(index, 1, this.emptyEntity);

            this.candidates = this.candidates.slice();
            this.selectedParticipants = this.selectedParticipants.slice();
        }
    }
}
