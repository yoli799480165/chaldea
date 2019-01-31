import { Component, Injector, OnInit } from '@angular/core';
import { ComponentBase } from 'app/shared/component-base';
import { NodeServiceProxy, ExtractFileDto } from 'shared/service-proxies/service-proxies';

@Component({
    selector: 'app-extract-file',
    templateUrl: './extract-file.component.html',
    styleUrls: ['./extract-file.component.scss']
})
export class ExtractFileComponent extends ComponentBase implements OnInit {
    extractFileDto = new ExtractFileDto();
    nodeId: string;

    constructor(
        private injector: Injector,
        private nodeServiceProxy: NodeServiceProxy
    ) {
        super(injector);
        this.extractFileDto.password = 'www.yxdm.tv';
    }

    ngOnInit() {
        setTimeout(() => {
            if (this.modal.data) {
                this.nodeId = this.modal.data.nodeId;
                this.extractFileDto.files = this.modal.data.files;
                this.extractFileDto.destDir = this.modal.data.destDir;
            }
        }, 0);
    }

    extract(): void {
        this.loading.show();
        this.nodeServiceProxy.extractFiles(this.nodeId, this.extractFileDto).subscribe((msg) => {
            this.modal.dismiss(msg);
            this.loading.hide();
        });
    }
}
