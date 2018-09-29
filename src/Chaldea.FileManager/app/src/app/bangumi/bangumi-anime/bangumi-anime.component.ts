import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AppConsts } from '../../../shared/AppConsts';
import { Anime } from '../../../shared/chaldea/chaldea-proxies';

@Component({
  selector: 'app-bangumi-anime',
  templateUrl: './bangumi-anime.component.html',
  styleUrls: ['./bangumi-anime.component.scss']
})
export class BangumiAnimeComponent implements OnInit {
  @Input()
  animes: Anime[] = [];

  @Output()
  showDetail = new EventEmitter<Anime>();

  coverUrl = `${AppConsts.chaldeaBaseUrl}/statics/imgs/covers/`;

  constructor(
  ) {

  }

  ngOnInit() {
  }
}
