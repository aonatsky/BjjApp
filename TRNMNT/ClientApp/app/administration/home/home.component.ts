import { DataService } from '../../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    readmeHtml: string;

    constructor(private dataService: DataService, private router: Router) {

    }

    ngOnInit() {
        this.dataService.getStaticContent("readme.md.html").subscribe(data => this.processHtml(data));
    }

    private processHtml(data) {
        this.readmeHtml = data.text();
    }



}
