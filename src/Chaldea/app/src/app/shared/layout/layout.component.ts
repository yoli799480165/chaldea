import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';

import { LoadingService } from '../loading-service';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html'
})
export class LayoutComponent implements OnInit {
    loading = false;

    constructor(
        public location: Location,
        private loadingService: LoadingService
    ) {
        this.loadingService.subject.subscribe((rep: boolean) => {
            this.loading = rep;
        });
    }

    ngOnInit() {
    }
}
