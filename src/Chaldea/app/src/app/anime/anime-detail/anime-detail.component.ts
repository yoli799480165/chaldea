import { Component, OnInit, Injector } from '@angular/core';
import { AnimeServiceProxy, AnimeDto, AnimeTagServiceProxy, AnimeTagDto } from '../../../shared/service-proxies/service-proxies';
import { ComponentBase } from '../../shared/component-base';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '../../../shared/AppConsts';

@Component({
  selector: 'app-anime-detail',
  templateUrl: './anime-detail.component.html',
  styleUrls: ['./anime-detail.component.scss']
})
export class AnimeDetailComponent extends ComponentBase implements OnInit {
  coverUrl: string;
  anime: AnimeDto = new AnimeDto();
  animeTag: AnimeTagDto = new AnimeTagDto();

  constructor(
    private injector: Injector,
    private activeRoute: ActivatedRoute,
    private animeServiceProxy: AnimeServiceProxy,
    private animeTagServiceProxy: AnimeTagServiceProxy
  ) {
    super(injector);
    this.coverUrl = `${AppConsts.appBaseUrl}/statics/imgs/`
  }

  ngOnInit() {
    this.getAnimeTags();
    this.getAnime();
  }

  getAnime(): void {
    const animeId = this.activeRoute.snapshot.params['animeId'];
    this.animeServiceProxy.getAnime(animeId).subscribe((rep) => {
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
}
