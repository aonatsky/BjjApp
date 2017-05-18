import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';


@Component({
    selector: 'crud',
    templateUrl: "./crud.component.html",
    styleUrls: ['./crud.component.css']
})


export class CrudComponent implements OnInit {

    ngOnInit(): void {

    }

    @Input() entities: any[] = [];
    @Input() title: string = "";
    @Input() columns: CrudColumn[] = [];
    @Input() editEnabled: boolean = true;
    @Input() deleteEnabled: boolean = true;

    @Output() onAdd: EventEmitter<any> = new EventEmitter<any>();
    @Output() onUpdate: EventEmitter<any> = new EventEmitter<any>();
    @Output() onDelete: EventEmitter<any> = new EventEmitter<any>();



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
        if (this.newEntity) {
            this.onAdd.emit(this.entityToEdit)
        } else {
            this.onUpdate.emit(this.entityToEdit)
        }
        this.displayDialog = false;
    }

    delete() {
        this.onDelete.emit(this.entityToEdit)
        this.displayDialog = false;
    }

    onRowSelect(event) {
        if (this.editEnabled || this.deleteEnabled) {
            this.showDialogToEdit(event.data)
        }
    }

    isIdColumn(column: CrudColumn): boolean {
        return column.propertyName.endsWith('Id');
    }
}

export interface CrudColumn {
    displayName: string;
    propertyName: string;
    isEditable: boolean;
}
