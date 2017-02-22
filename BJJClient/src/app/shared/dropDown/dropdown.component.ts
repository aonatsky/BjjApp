import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core'


@Component({
  selector: 'dropdown',
  templateUrl: "./dropdown.component.html",
  styleUrls: ['./dropdown.component.css']
})


export class DropdownComponent implements OnInit {
  @Input() dropdownValues: any[];
  @Input() nameProperty: string;
  @Input() idProperty: string;

  @Output() onSelect: EventEmitter<any>;

  selectedValue: any;

  constructor() {
    this.onSelect = new EventEmitter<any>();
  }

  ngOnInit() {
    this.selectedValue = this.dropdownValues[0];
  }

  selectItem(value) {
    this.selectedValue = value;
    this.onSelect.emit(value);
  }
}

export /**
 * DropDownListOption
 */
  class DropDownListOption {
  constructor(private id: any, private name: string) {

  }
}