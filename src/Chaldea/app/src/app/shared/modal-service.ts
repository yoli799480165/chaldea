import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Subject } from 'rxjs/Rx';

@Injectable()
export class ModalService {
    modalRef: BsModalRef;
    modalSubject: Subject<any>;
    data: any;
    config = {
        backdrop: true,
        ignoreBackdropClick: true,
        class: 'modal-md'
    };

    constructor(
        private modalService: BsModalService
    ) {
    }

    show(component: any, data?: any, cfg?: any): Subject<any> {
        const self = this;
        self.modalSubject = new Subject<any>();
        self.modalRef = self.modalService.show(component, cfg ? cfg : this.config);
        self.data = data;
        return self.modalSubject;
    }

    close(): void {
        if (this.data) {
            this.data = undefined;
        }

        if (this.modalSubject) {
            this.modalSubject = undefined;
        }

        this.modalRef.hide();
    }

    getData<T>(): T {
        return <T>this.data;
    }

    dismiss(data?: any): void {
        this.modalSubject.next(data);
        this.close();
    }
}
