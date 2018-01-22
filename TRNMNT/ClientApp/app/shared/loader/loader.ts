import { Component, Input } from '@angular/core';
import { animate, state, trigger, transition, style } from '@angular/animations';
import './loader.scss';

@Component({
    selector: 'loader',
    animations: [
        trigger('visibilityChanged', [
            state('true', style({ opacity: 0.9 })),
            state('false', style({ opacity: 0, display: 'none' })),
            transition('1 => 0', animate('0.01s')),
            transition('0 => 1', animate('0.5s'))
        ])
    ],
    templateUrl: './loader.html'
})
export class LoaderComponent {
    @Input() isVisible: boolean = true;
}