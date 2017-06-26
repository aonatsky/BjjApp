import { DataService } from '../../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
    selector: 'topbar',
    templateUrl: './topbar.component.html'
})
export class TopbarComponent implements OnInit {

    constructor(private dataService: DataService, private router: Router) {

    }

    ngOnInit() {
        
    }


}
