import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AppConsts } from '../../../shared/AppConsts';
import {
  AnimeDto,
  AnimeServiceProxy,
  AnimeTagDto,
  AnimeTagServiceProxy
} from '../../../shared/service-proxies/service-proxies';
import { ComponentBase } from '../../shared/component-base';

@Component({
  selector: 'app-anime-detail',
  templateUrl: './anime-detail.component.html',
  styleUrls: ['./anime-detail.component.scss']
})
export class AnimeDetailComponent extends ComponentBase implements OnInit {
  cover = '';
  anime: AnimeDto = new AnimeDto();
  animeTag: AnimeTagDto = new AnimeTagDto();

  constructor(
    private injector: Injector,
    private activeRoute: ActivatedRoute,
    private animeServiceProxy: AnimeServiceProxy,
    private animeTagServiceProxy: AnimeTagServiceProxy
  ) {
    super(injector);
    this.anime.tags = [];
  }

  ngOnInit() {
    this.getAnimeTags();
    this.getAnime();
  }

  getAnime(): void {
    const animeId = this.activeRoute.snapshot.params['animeId'];
    this.animeServiceProxy.getAnime(animeId).subscribe((rep) => {
      rep.desc = this.escape(rep.desc);
      this.cover = `${AppConsts.appBaseUrl}/statics/imgs/cover/${rep.cover}`;
      this.anime = rep;
    });
  }

  getAnimeTags(): void {
    this.animeTagServiceProxy.getTags().subscribe((rep) => {
      this.animeTag = rep;
    });
  }

  selectTag(tag: string): void {
    const self = this;
    const index = self.anime.tags.indexOf(tag);
    if (index > -1) {
      self.anime.tags.splice(index, 1);
    } else {
      self.anime.tags.push(tag);
    }
  }

  save(): void {
    this.animeServiceProxy.update(this.anime).subscribe(() => {
      this.showSuccess('保存成功');
    });
  }

  removeVideo(sourceId: string): void {
    this.dialog.confirm('确定删除该资源?').subscribe((confirm) => {
      if (confirm) {
        this.animeServiceProxy.removeVideos(this.anime.id, [sourceId]).subscribe(() => {
          this.getAnime();
        });
      }
    });
  }

  escape(value) {
    if (value) {
      return value
        .replace(new RegExp('&ldquo;', 'g'), '“')
        .replace(new RegExp('&rdquo;', 'g'), '”')
        .replace(new RegExp('&mdash;', 'g'), '—')
        .replace(new RegExp('&hellip;', 'g'), '…')
        .replace(new RegExp('&middot;', 'g'), '·')
        .replace(new RegExp('&times;', 'g'), '×')
        .replace(new RegExp('&quot;', 'g'), '"')
        .replace(new RegExp('&amp;', 'g'), '&')
        .replace(new RegExp('&lsquo;', 'g'), '‘')
        .replace(new RegExp('&rsquo;', 'g'), '’')
        .replace(new RegExp('&nbsp;', 'g'), ' ')
        .replace(new RegExp('&#39;', 'g'), '\'')
        .replace(new RegExp('收缩全文', 'g'), '');
    }
    return value;
  }
}
