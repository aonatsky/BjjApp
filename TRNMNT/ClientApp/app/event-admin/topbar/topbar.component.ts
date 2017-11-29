import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { RouterService } from './../../core/services/router.service';
import { AuthService } from './../../core/services/auth.service';
import { UserModel } from './../../core/model/user.model';


@Component({
    selector: 'topbar',
    templateUrl: './topbar.component.html',
    styleUrls: ['./topbar.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class TopbarComponent  implements OnInit {

    user: UserModel;

    constructor(private routerService: RouterService, private authService: AuthService) {

    }

    ngOnInit() {
        this.user = this.authService.getUser();
    }


}
