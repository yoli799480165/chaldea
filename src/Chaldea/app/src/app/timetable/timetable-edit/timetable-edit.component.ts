import { Component, OnInit, Injector } from '@angular/core';
import { AnimeServiceProxy, AnimeOutlineDto, TimetableServiceProxy, Timetable, DropdownItem } from 'shared/service-proxies/service-proxies';
import { ComponentBase } from 'app/shared/component-base';
import moment = require('moment');

@Component({
  selector: 'app-timetable-edit',
  templateUrl: './timetable-edit.component.html',
  styleUrls: ['./timetable-edit.component.scss']
})
export class TimetableEditComponent extends ComponentBase implements OnInit {
  animes: AnimeOutlineDto[] = [];
  selectAnimes: AnimeOutlineDto[];
  weeks: DropdownItem[] = [];
  selectWeek: DropdownItem;
  input: Timetable = new Timetable();
  selectDate: moment.Moment[] = [];
  config = {
    displayKey: 'title',
    search: true,
    height: 'auto',
    placeholder: '番剧列表',
    moreText: '更多',
    noResultsFound: '对找到对应的资源!',
    searchPlaceholder: '搜索',
    limitTo: 10
  };
  bsConfig = {
    rangeInputFormat: 'YYYY-MM-DD',
    containerClass: 'theme-default'
  };

  constructor(
    private injector: Injector,
    private animeServiceProxy: AnimeServiceProxy,
    private timetableServiceProxy: TimetableServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getAnimes();
    this.getWeeks();
  }


  getAnimes(): void {
    this.animeServiceProxy.getList('', 0, 0).subscribe((rep) => {
      this.animes = rep;
    });
  }

  getWeeks(): void {
    this.timetableServiceProxy.getWeeks().subscribe((rep) => {
      this.weeks = rep;
      this.selectWeek = rep[0];
    });
  }

  save(): void {
    this.loading.show();
    if (this.selectAnimes.length <= 0) {
      this.dialog.alert('请选择番剧');
      return;
    }
    this.input.animeId = this.selectAnimes[0].id;
    this.input.beginDate = this.selectDate[0];
    this.input.endDate = this.selectDate[1];
    this.input.updateWeek = +this.selectWeek.value;
    this.timetableServiceProxy.createTimetable(this.input).subscribe(() => {
      this.loading.hide();
      // this.modal.dismiss(true);
    });
  }
}
