import { Component, OnInit, Injector } from '@angular/core';
import { AnimeServiceProxy, AnimeOutlineDto, BangumiServiceProxy, BangumiDto } from '../../shared/service-proxies/service-proxies';
import { AppConsts } from 'shared/AppConsts';
import { ComponentBase } from '../shared/component-base';
import { AnimeImportComponent } from './anime-import/anime-import.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-anime',
  templateUrl: './anime.component.html',
  styleUrls: ['./anime.component.scss']
})
export class AnimeComponent extends ComponentBase implements OnInit {
  coverUrl: string;
  bangumis: BangumiDto[] = [];
  animes: AnimeOutlineDto[] = [];
  selectBangumi: BangumiDto;

  constructor(
    private injector: Injector,
    private router: Router,
    private bangumiServiceProxy: BangumiServiceProxy,
    private animeServiceProxy: AnimeServiceProxy
  ) {
    super(injector);
    this.coverUrl = `${AppConsts.appBaseUrl}/statics/imgs/`
  }

  ngOnInit() {
    this.getBangumiList();
  }

  getBangumiList(): void {
    this.bangumiServiceProxy.getList().subscribe((rep) => {
      const bangumi = new BangumiDto();
      bangumi.id = '';
      bangumi.name = '显示全部';
      rep.unshift(bangumi);
      this.bangumis = rep;
      this.selectBangumi = bangumi;
      this.getList();
    });
  }

  getList(): void {
    this.animeServiceProxy.getList(this.selectBangumi.id, 0, 0).subscribe((rep) => {
      this.animes = rep;
    });
  }

  import(): void {
    if (this.selectBangumi.id === '') {
      this.dialog.alert('请选择番组!');
      return;
    }
    this.modal.show(AnimeImportComponent, this.selectBangumi.id).subscribe(() => {
      this.getList();
    });
  }

  showDetail(anime: AnimeOutlineDto): void {
    this.router.navigate(['/anime-detail', anime.id]);
  }
}
