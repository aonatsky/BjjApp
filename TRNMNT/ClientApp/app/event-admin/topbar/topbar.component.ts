import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { RouterService } from './../../core/services/router.service';
import { AuthService } from './../../core/services/auth.service';
import { UserModel } from './../../core/model/user.model';

import './topbar.component.scss';
import { MenuItem } from 'primeng/components/common/menuitem';

@Component({
    selector: 'topbar',
    templateUrl: './topbar.component.html',
    encapsulation: ViewEncapsulation.None
})
export class TopbarComponent  implements OnInit {

    user: UserModel;
    items: MenuItem[];

    constructor(private routerService: RouterService, private authService: AuthService) {

    }

    ngOnInit() {
        this.user = this.authService.getUser();
        const isLoggedIn = this.authService.isLoggedIn();
        this.items = [
            {
                label: 'Home',
                routerLink: '/'
            },
            //{
            //    label: 'Events',
            //    routerLink: '/'
            //},
            //{
            //    label: 'About',
            //    routerLink: '/'
            //},
            {
                label: isLoggedIn ? 'Logout': 'Login',
                command: (event) => {
                    if (this.authService.isLoggedIn()) {
                        this.authService.signout();
                    }
                    this.routerService.goToLogin();
                }
            }
        ];
    }

    private goHome() {
        this.routerService.goHome();
    }

}
