﻿import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'minuteSeconds'
})
export class MinuteSecondsPipe implements PipeTransform {

    transform(value: number): string {
        const minutes = Math.floor(value / 60);
        let seconds = value - minutes * 60;
        return (minutes < 10 ? '0' : '' ) + minutes + ':' + (seconds < 10 ? '0' : '') + seconds;
    }

}