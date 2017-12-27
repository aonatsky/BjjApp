import { OnInit, Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { LoaderService } from '../../core/services/loader.service'
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

@Component({
    selector: 'crud',
    templateUrl: "./crud.component.html",
    styleUrls: ['./crud.component.css']
})


export class CrudComponent implements OnInit, OnChanges {

    constructor(private loaderService: LoaderService) {
        this.onAdd = new EventEmitter<any>();
        this.onUpdate = new EventEmitter<any>();
        this.onDelete = new EventEmitter<any>();
        this.onLazyLoad = new EventEmitter<LazyLoadEvent>();
    }

    ngOnInit(): void {
        this.entities = [];
        this.loaderService.showLoader();
    }

    ngOnChanges(): void {
        this.loaderService.hideLoader();
    }

    @Input() entities: any[] = [];
    @Input() title: string = "";
    @Input() columns: CrudColumn[] = [];
    @Input() editEnabled: boolean = true;
    @Input() addEnabled: boolean = true;
    @Input() deleteEnabled: boolean = true;
    @Input() showRowNumbers: boolean = true;
    @Input() lazy: boolean = false;
    @Input() pageSize: number = 10;
    @Input() totalRecords: number = 0;

    @Output() onAdd: EventEmitter<any>;
    @Output() onUpdate: EventEmitter<any>;
    @Output() onDelete: EventEmitter<any>;
    @Output() onLazyLoad: EventEmitter<LazyLoadEvent>;
    


    displayDialog: boolean;
    newEntity: boolean;
    selectedEntity: any;
    entityToEdit: any = new Object();


    showDialogToAdd() {
            this.newEntity = true;
            this.entityToEdit = new Object();
            this.displayDialog = true;
    }

    showDialogToEdit(entity: any) {
        this.newEntity = false;
        this.entityToEdit = entity;
        this.displayDialog = true;
    }

    save() {
        this.loaderService.showLoader();
        if (this.newEntity) {
            this.onAdd.emit(this.entityToEdit);
        } else {
            this.onUpdate.emit(this.entityToEdit);
        }
        this.displayDialog = false;
    }

    delete() {
        this.loaderService.showLoader();
        this.onDelete.emit(this.entityToEdit);
        this.displayDialog = false;
    }

    onRowSelect(event) {
        if (this.editEnabled || this.deleteEnabled) {
            this.showDialogToEdit(event.data);
        }
    }

    lazyLoadData($event: LazyLoadEvent) {
        this.onLazyLoad.emit($event);
    }

    isIdColumn(column: CrudColumn): boolean {
        return column.propertyName.endsWith('Id');
    }
}

export interface CrudColumn {
    displayName: string;
    propertyName: string;
    isEditable: boolean;
    isSortable: boolean;
    useClass?: (value: any) => string;
    transform?: (value: any) => string;
}
