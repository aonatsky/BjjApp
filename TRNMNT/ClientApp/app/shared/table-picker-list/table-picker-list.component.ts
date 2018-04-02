import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { BracketService } from '../../core/services/bracket.service';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { RouterService } from '../../core/services/router.service';
import './table-picker-list.component.scss'

@Component({
    selector: 'table-picker-list',
    templateUrl: './table-picker-list.component.html',
})
export class TablePickerListComponent implements OnInit, OnChanges {

    @Input() sourceArray: any[] = [];
    @Input() targetArray: any[] = [];
    @Input() readonly columnsData: IPickerColumn[] = [];
    @Input() readonly sortField: string;
    @Input() readonly sortDirection: number = 1;

    private targetArrayInternal: any[] = [];
    private sourceArrayInternal: any[] = [];
    private selectedSourceEntity: any;
    private selectedTargetEntity: any;
    private emptyProto: any;
    private index: number = 0;
    private sortDirectionBack: number;

    private get originalEntityCount(): number {
        return this.sourceArray.length;
    }

    private get emptyEntity(): any {
        let clone: any = Object.assign({}, this.emptyProto);
        clone.__index = this.index++;
        return clone;
    }

    constructor(
        private loggerService: LoggerService,
        private bracketService: BracketService,
        private route: ActivatedRoute,
        private routerService: RouterService,
        private runEventHubService: RunEventHubService) {
    }

    ngOnInit() {
        this.sortDirectionBack = this.sortDirection === 1 ? 0 : 1;
        this.emptyProto = {
            __index: 0,
            __empty: true
        };
        for (let column of this.columnsData) {
            this.emptyProto[column.propertyName] = "\u00a0";
        }
    }

    ngOnChanges(changes: SimpleChanges): void {
        const sourcesChanges = changes["sourceArray"];
        if (!!sourcesChanges.currentValue && sourcesChanges.currentValue.length > 0) {
            this.targetArrayInternal = this.targetArray.slice();
            this.sourceArrayInternal = this.sourceArray.slice();
            this.populateArray(this.targetArrayInternal);
        }
    }

    private addItem() {
        this.pickEntity(this.selectedSourceEntity, this.sourceArrayInternal, this.targetArrayInternal);
        if (!!this.selectedSourceEntity) {
            this.selectedTargetEntity = this.selectedSourceEntity;
            const next = this.sourceArrayInternal.find(p => !p.__empty);
            this.selectedSourceEntity = next;
        }
    }

    private removeItem() {
        this.pickEntity(this.selectedTargetEntity, this.targetArrayInternal, this.sourceArrayInternal);
        if (!!this.selectedTargetEntity) {
            this.selectedSourceEntity = this.selectedTargetEntity;
            const next = this.targetArrayInternal.find(p => !p.__empty);
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
        if (!!entity && !entity.__empty) {
            const index = sourceArray.findIndex(p => p === entity);
            const targetIndex = targetArray.findIndex(p => !!p.__empty);
            targetArray.splice(targetIndex, 1, entity);
            sourceArray.splice(index, 1, this.emptyEntity);

            this.sourceArrayInternal = this.sourceArrayInternal.slice();
            this.targetArrayInternal = this.targetArrayInternal.slice();
            this.sourceArray = this.sourceArrayInternal.filter(x => !x.__empty);
            this.targetArray = this.targetArrayInternal.filter(x => !x.__empty);
        }
    }
}

export interface IPickerColumn {
    displayName: string;
    propertyName: string;
    isSortable: boolean;
}
