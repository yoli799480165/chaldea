import { Injector } from '@angular/core';
import { ModalService } from 'app/shared/modal-service';
import { NgxCoolDialogsService } from 'ngx-cool-dialogs';

declare var $: any;

export class ComponentBase {
  public dialog: NgxCoolDialogsService;
  public modal: ModalService;

  constructor(injector: Injector) {
    this.modal = injector.get(ModalService);
    this.dialog = injector.get(NgxCoolDialogsService);
  }

  showTips(msg: string, type: string): void {
    // 'info', 'success', 'warning', 'danger'
    const color = Math.floor((Math.random() * 4) + 1);
    $.notify({
      icon: 'pe-7s-gift',
      message: msg
    }, {
        type: type,
        timer: 1000,
        placement: {
          from: 'top',
          align: 'center'
        }
      });
  }

  showSuccess(msg: string): void {
    this.showTips(msg, 'success');
  }

  showError(msg: string): void {
    this.showTips(msg, 'danger');
  }

  getStateDesc(state: number): string {
    if (state === 0) {
      return `<span style="color:red;">未运行</span>`;
    }
    if (state === 1) {
      return `<span style="color:green;">运行中</span>`;
    }
  }
}
