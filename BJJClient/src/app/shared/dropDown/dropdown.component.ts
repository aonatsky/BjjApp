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
  templateUrl: "./dropdown.component.html"
})
export class DropdownComponent {
  @Input() dropDownValues;

  @Output()
  select: EventEmitter<string>;

  constructor() {
    this.select = new EventEmitter();
  }

  selectItem(value) {
    this.select.emit(value);
  }
}