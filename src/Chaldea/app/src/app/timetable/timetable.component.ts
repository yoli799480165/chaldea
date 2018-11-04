import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from 'app/shared/component-base';
import { TimetableEditComponent } from './timetable-edit/timetable-edit.component';
import { TimetableServiceProxy, TimetableDto } from 'shared/service-proxies/service-proxies';
import { ClipboardService } from 'ngx-clipboard';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.scss']
})
export class TimetableComponent extends ComponentBase implements OnInit {
  timetables: TimetableDto[] = [];

  constructor(
    private injector: Injector,
    private timetableServiceProxy: TimetableServiceProxy,
    private clipboardService: ClipboardService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getList();
  }

  getList(): void {
    this.timetableServiceProxy.getList(0, 0).subscribe((rep) => {
      this.timetables = rep;
    });
  }

  edit(): void {
    this.modal.show(TimetableEditComponent).subscribe((rep) => {
      if (rep) {
        this.getList();
      }
    });
  }

  copyPwd(pwd: string): void {
    this.clipboardService.copyFromContent(pwd);
  }
}
