import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';


@Component({
    selector: 'crud',
    templateUrl: "./crud.component.html",
    styleUrls: ['./crud.component.css']
})


export class CrudComponent implements OnInit {

    ngOnInit(): void {

    }

    @Input() entities: any[]
    @Input() title:string = "";
    @Input() columns: CrudColumn[];
    
    displayDialog: boolean;
    newEntity: boolean;
    selectedEntity: any;
    entity: any = new Object();


    showDialogToAdd() {
        this.newEntity = true;
        this.entity = new Object();
        this.displayDialog = true;
    }

    save() {
        this.displayDialog = false;
    }

    delete() {
        this.displayDialog = false;
    }

    onRowSelect(event) {
        this.showDialogToAdd();
    }

    cloneEntity(e: any): any {
        let newEntity = new Object();
        for (let prop in e) {
            newEntity[prop] = e[prop];
        }
        return newEntity;
    }
   
}

export interface CrudColumn{
    displayName: string;
    propertyName: string;
}
