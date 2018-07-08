import { Component, OnInit, Input } from '@angular/core';

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
  @Input() readonly dialogWidth?: number;
  @Input() readonly dialogHeight?: number;
  @Input() readonly dialogMinWidth: number = 200;
  @Input() readonly dialogMinHeight: number = 350;
  @Input() visible: boolean;

  close(){
    this.visible = true;
  }


  ngOnInit() {
    this.showHeader = !!this.title;
  }
}
