import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'dictionary' })
export class DictionaryPipe implements PipeTransform {
    transform(object, args: string[]): any {
        const items = [];
        for (const key in object) {
            if (object.hasOwnProperty(key)) {
                items.push({ key: key, value: object[key] });
            }
        }
        return items;
    }
}
