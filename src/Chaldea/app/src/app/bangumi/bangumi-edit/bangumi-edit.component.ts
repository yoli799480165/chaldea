import { Component, OnInit, Injector } from '@angular/core';
import { BangumiEditDto, BangumiServiceProxy } from '../../../shared/service-proxies/service-proxies';
import { ComponentBase } from '../../shared/component-base';

@Component({
  selector: 'app-bangumi-edit',
  templateUrl: './bangumi-edit.component.html',
  styleUrls: ['./bangumi-edit.component.scss']
})
export class BangumiEditComponent extends ComponentBase implements OnInit {
  input: BangumiEditDto = new BangumiEditDto;

  constructor(
    private injector: Injector,
    private bangumiServiceProxy: BangumiServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    setTimeout(() => {
      if (this.modal.data) {
        this.input.id = this.modal.data.id;
        this.input.name = this.modal.data.name;
      }
    }, 0);
  }

  save(): void {
    this.bangumiServiceProxy.createOrUpdate(this.input).subscribe(() => {
      this.modal.dismiss(true);
    });
  }
}
