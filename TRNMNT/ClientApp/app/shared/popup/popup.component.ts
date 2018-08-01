import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-popup',
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.scss']
})
export class PopupComponent implements OnInit {
  constructor() {}
  @Input() title: string;
  @Input() showHeader: boolean;
  @Input() closable: boolean;
  @Input() readonly dialogWidth?: number = 500;
  @Input() readonly dialogHeight?: number;
  @Input() readonly dialogMinWidth: number = 200;
  @Input() readonly dialogMinHeight: number = 350;
  @Input() visible: boolean = false;

  @Output() onHide: EventEmitter<any> = new EventEmitter();
  @Output() onShow: EventEmitter<any> = new EventEmitter();

  close(){
    this.visible = true;
  }


  ngOnInit() {
    this.showHeader = !!this.title;
  }
}
