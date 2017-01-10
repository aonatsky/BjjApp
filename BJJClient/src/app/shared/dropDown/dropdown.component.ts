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
  @Input() dropDownValues : any;
  @Output() onSelect : EventEmitter<string> = new EventEmitter<string>();

  constructor() {
    this.onSelect = new EventEmitter<string>();
  }

  selectItem(value) {
    this.onSelect.emit(value);
  }
}