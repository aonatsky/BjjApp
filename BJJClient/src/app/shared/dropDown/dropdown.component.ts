import{Component, Input, Output, EventEmitter} from '@angular/core'

// export class DropdownValue {
//   value:string;
//   label:string;

//   constructor(value:string,label:string) {
//     this.value = value;
//     this.label = label;
//   }
// }

@Component({
  selector: 'dropdown',
  templateUrl: "./dropdown.component.html"
})


export class DropdownComponent {
  @Input() dropdownValues : any;
  @Input() nameProperty : string;
  @Input() idProperty: string;
  
  @Output() onSelect : EventEmitter<string> = new EventEmitter<string>();

  constructor() {
    this.onSelect = new EventEmitter<string>();
  }

  selectItem(value) {
    this.onSelect.emit(value);
  }
}