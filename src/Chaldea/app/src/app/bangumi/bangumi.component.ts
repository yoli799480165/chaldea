import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '../shared/component-base';
import { BangumiServiceProxy, BangumiDto } from '../../shared/service-proxies/service-proxies';
import { BangumiEditComponent } from './bangumi-edit/bangumi-edit.component';

@Component({
  selector: 'app-bangumi',
  templateUrl: './bangumi.component.html',
  styleUrls: ['./bangumi.component.scss']
})
export class BangumiComponent extends ComponentBase implements OnInit {
  bangumis: BangumiDto[] = [];

  constructor(
    private injector: Injector,
    private bangumiServiceProxy: BangumiServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getList();
  }

  getList(): void {
    this.bangumiServiceProxy.getList().subscribe((resp) => {
      this.bangumis = resp;
    });
  }

  create(): void {
    this.edit();
  }

  edit(bangumi?: BangumiDto): void {
    this.modal.show(BangumiEditComponent, bangumi).subscribe((rep) => {
      this.getList();
    });
  }

  delete(id: string): void {
    this.dialog.confirm('确定要删除?').subscribe(res => {
      if (res) {
        this.bangumiServiceProxy.delete(id).subscribe(() => {
          this.getList();
        });
      }
    });
  }
}
