import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AnimeServiceProxy, Anime, Bangumi } from '../../shared/chaldea/chaldea-proxies';
import { AppConsts } from '../../shared/AppConsts';

@Component({
  selector: 'app-bangumi',
  templateUrl: './bangumi.component.html',
  styleUrls: ['./bangumi.component.scss']
})
export class BangumiComponent implements OnInit {
  listType = 'bangumi';
  selectBangumi: Bangumi;
  selectAnime: Anime;
  selectPath = '';

  constructor(
    private _location: Location
  ) { }

  ngOnInit() {
  }

  goHome(): void {
    this.listType = 'bangumi';
    this.selectPath = '';
  }

  back(): void {
    switch (this.listType) {
      case 'bangumi':
        break;
      case 'anime':
        this.listType = 'bangumi';
        break;
      case 'anime-detail':
        this.listType = 'anime';
        break;
    }
  }

  forward(): void {
  }

  showAnimes(bangumi: Bangumi): void {
    this.listType = 'anime';
    this.selectBangumi = bangumi;
    this.selectPath = `${this.selectBangumi.name} >`;
  }

  showDetail(anime: Anime): void {
    this.listType = 'anime-detail';
    this.selectAnime = anime;
    this.selectPath = `${this.selectBangumi.name} > ${this.selectAnime.title} >`;
  }
}
