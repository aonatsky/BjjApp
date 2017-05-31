import {Component, Input} from '@angular/core';


@Component({
    selector: 'static-html',
    templateUrl: "./static-html.component.html",
    styleUrls: ['./static-html.component.css']
})


export class StaticHtmlComponent {
    @Input() body: string;
}
