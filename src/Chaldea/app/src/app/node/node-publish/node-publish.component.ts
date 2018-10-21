import { Component, Injector, OnInit } from '@angular/core';
import { ComponentBase } from 'app/shared/component-base';
import {
  AnimeOutlineDto,
  AnimeServiceProxy,
  NodeServiceProxy,
  PublishResourceDto,
  PublishDirFileInfo
} from 'shared/service-proxies/service-proxies';

@Component({
  selector: 'app-node-publish',
  templateUrl: './node-publish.component.html',
  styleUrls: ['./node-publish.component.scss']
})
export class NodePublishComponent extends ComponentBase implements OnInit {
  animes: AnimeOutlineDto[] = [];
  selectAnimes: AnimeOutlineDto[];
  selectDirFiles: PublishDirFileInfo[] = [];
  displayFormat = '第{0}话';
  nodeId: string;
  urlPrefix = '';

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

  constructor(
    private injector: Injector,
    private animeServiceProxy: AnimeServiceProxy,
    private nodeServiceProxy: NodeServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getAnimes();
    setTimeout(() => {
      if (this.modal.data) {
        this.nodeId = this.modal.data.nodeId;
        this.urlPrefix = this.modal.data.selectPath.replace(/\\/g, '/');
        const items = <PublishDirFileInfo[]>this.modal.data.selectDirFiles.map((x, index) => {
          const item = new PublishDirFileInfo();
          item.displayName = this.getDisplayName(this.displayFormat, index);
          item.url = `${this.urlPrefix}/${item.name}`;
          item.fullName = x.fullName;
          item.length = x.length;
          item.modifyTime = x.modifyTime;
          item.name = x.name;
          item.size = x.size;
          item.type = x.type;
          return item;
        });
        this.selectDirFiles = items;
      }
    }, 0);
  }

  getAnimes(): void {
    this.animeServiceProxy.getList('', 0, 0).subscribe((rep) => {
      this.animes = rep;
    });
  }

  format($event): void {
    this.selectDirFiles.forEach((item, index) => {
      item.displayName = this.getDisplayName($event, index);
    });
  }

  setUrl($event): void {
    this.selectDirFiles.forEach((item) => {
      item.url = `${$event}/${item.name}`;
    });
  }

  publish(): void {
    if (!this.selectAnimes || this.selectAnimes.length <= 0) {
      this.dialog.alert('请选择番组!');
      return;
    }
    this.loading.show();
    const input = new PublishResourceDto();
    input.animeId = this.selectAnimes[0].id;
    input.publishFiles = this.selectDirFiles;
    this.nodeServiceProxy.publishResource(this.nodeId, input).subscribe(() => {
      this.loading.hide();
      this.modal.dismiss(true);
    }, err => {
      this.loading.hide();
    });
  }

  getDisplayName(displayFormat: string, index: number): string {
    const num = +displayFormat.match(/\d/g);
    return displayFormat.replace(/{\d}/g, (index + num + 1).toString().padStart(2, '0'));
  }
}
