import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';


@Component({
    selector: 'crud',
    templateUrl: "./crud.component.html",
    styleUrls: ['./crud.component.css']
})


export class CrudComponent implements OnInit {

    ngOnInit(): void {
        for(let key in this.entities[0]){
            // this.properties.push(key);
            console.log(key);
        }
    }



    @Input() entities: any[]

    displayDialog: boolean;
    newEntity: boolean;
    selectedEntity: any;
    entity: any = new Object();

    properties: string[];

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