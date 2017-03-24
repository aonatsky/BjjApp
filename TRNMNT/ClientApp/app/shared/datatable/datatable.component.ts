import { Input, Component } from '@angular/core';
import { ColumnComponent } from "../column/column.component";

@Component({
    selector: "datatable",
    templateUrl:"./datatable.component.html",
    styleUrls:['./datatable.component.css']
})
export class DataTableComponent {
    constructor() {

    }

  @Input() dataset;
  @Input() title;
  @Input() isCrud = true;


  columns: ColumnComponent[] = [];
 
  addColumn(column){
    this.columns.push(column);
  }

  showDialogToAdd(){
  }
}