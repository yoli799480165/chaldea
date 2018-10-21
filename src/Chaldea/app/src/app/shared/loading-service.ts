import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class LoadingService {
    public subject: Subject<boolean> = new Subject<boolean>();
    state = false;

    show(): void {
        if (!this.state) {
            // it will show before view init, need timeout
            setTimeout(() => {
                this.state = true;
                this.subject.next(this.state);
            }, 0);
        }
    }

    hide(): void {
        if (this.state) {
            this.state = false;
            this.subject.next(this.state);
        }
    }
}
