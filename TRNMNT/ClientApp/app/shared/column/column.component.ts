import {Component, Input} from '@angular/core';
import {DataTableComponent} from './../datatable/datatable.component';
 
@Component({
  selector: 'column',
  template: ``,
 
})
export class ColumnComponent {
	@Input() value;
	@Input() header;
 
	constructor(table: DataTableComponent) {
    	   table.addColumn(this)
  	}
}