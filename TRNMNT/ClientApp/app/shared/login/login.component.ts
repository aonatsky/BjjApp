
import { Component } from 'angular/core';

@Component({
    selector: 'login',
    template: 'Hello my name is {{name}}.'
})
export class LoginComponent {
    constructor() {
        this.name = 'Sam';
    }
}
