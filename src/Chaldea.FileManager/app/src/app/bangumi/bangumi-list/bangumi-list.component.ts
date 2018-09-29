import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BangumiServiceProxy, Bangumi, Anime } from '../../../shared/chaldea/chaldea-proxies';

@Component({
  selector: 'app-bangumi-list',
  templateUrl: './bangumi-list.component.html',
  styleUrls: ['./bangumi-list.component.scss']
})
export class BangumiListComponent implements OnInit {
  @Output()
  showAnimes = new EventEmitter<Bangumi>();

  bangumis: Bangumi[] = [];

  constructor(
    private bangumiServiceProxy: BangumiServiceProxy
  ) { }

  ngOnInit() {
    this.getBangumis();
  }


  getBangumis(): void {
    const self = this;
    self.bangumiServiceProxy.getlist(0, 0).subscribe((rep) => {
      self.bangumis = rep;
    });
  }
}
