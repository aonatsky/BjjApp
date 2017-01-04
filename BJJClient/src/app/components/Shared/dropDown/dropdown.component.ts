import{Component, Input, Output, EventEmitter} from '@angular/core'

export class DropdownValue {
  value:string;
  label:string;

  constructor(value:string,label:string) {
    this.value = value;
    this.label = label;
  }
}

@Component({
  selector: 'dropdown',
  template: require("./dropdown.component.html")
})
export class DropdownComponent {
  @Input() dropDownValues;
  //values: DropdownValue[];

  @Output()
  select: EventEmitter<any>;

  // constructor() {
  //   this.select = new EventEmitter();
  // }
 
  selectItem(value) {
    this.select.emit(value);
  }
}