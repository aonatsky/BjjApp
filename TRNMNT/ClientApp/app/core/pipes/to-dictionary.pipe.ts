import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'toDictionary'
})
export class ToDictionaryPipe implements PipeTransform {
    transform(value: Object): any {
        let simpleDictionary = [];
        for (let key in value) {
            if (value.hasOwnProperty(key)) {
                simpleDictionary.push({ key: key, value: value[key] });
            }
        }
        return simpleDictionary;
    }
}