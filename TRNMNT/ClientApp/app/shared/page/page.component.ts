import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'trnmnt-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {

  @Input() title : string;
  @Input() contentClass : string = "";
  constructor() { }

  ngOnInit() {
  }

}
