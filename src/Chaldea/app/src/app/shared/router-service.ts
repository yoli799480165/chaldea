import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable()
export class RouterService {
    map: { [key: string]: any } = {};
    constructor(
        private router: Router
    ) {

    }

    navigate(path: string, data: any): void {
        const key = new Date().valueOf().toString();
        this.map[key] = data;
        this.router.navigate([path, key]);
    }

    getData(key: string): any {
        return this.map[key];
    }
}
