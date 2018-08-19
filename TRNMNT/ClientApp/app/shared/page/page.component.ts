import { Component, OnInit, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'trnmnt-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {
  @Input()
  title: string;
  @Input()
  contentClass: string = '';
  @Input()
  useCommonContentStyle: boolean = true;
  constructor(private titleService: Title) {}

  ngOnInit() {
    this.titleService.setTitle(this.title);
  }

  getContentClass(): string {
    if (this.useCommonContentStyle) {
      return 'ui-widget ui-widget-content ' + this.contentClass;
    }
    return '';
  }
}
