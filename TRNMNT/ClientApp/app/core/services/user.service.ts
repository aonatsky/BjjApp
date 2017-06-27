import {Injectable} from '@angular/core'
import { AuthenticationService } from './authentication.service';

@Injectable()
export class UserService {
    constructor(private authenticationService: AuthenticationService) {

    }

    public getUser() {
        return this.authenticationService.getUser();
    }
}



