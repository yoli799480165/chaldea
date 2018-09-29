import { Component, Input, OnInit } from '@angular/core';

import { Anime, AnimeDetail, AnimeServiceProxy, Bangumi, Resource, MediaMetaData } from '../../../shared/chaldea/chaldea-proxies';
import {
  DirectoryServiceProxy,
  FileDto,
  GetFilesDto,
  RenameDirectoryDto,
  RenameFilesDto,
} from '../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'app-bangumi-anime-detail',
  templateUrl: './bangumi-anime-detail.component.html',
  styleUrls: ['./bangumi-anime-detail.component.scss']
})
export class BangumiAnimeDetailComponent implements OnInit {
  @Input()
  bangumi: Bangumi = new Bangumi();
  @Input()
  anime: Anime = new Anime();
  animeDetail: AnimeDetail = new AnimeDetail();
  drive = '/mnt/anime4/';
  path = '';
  files: FileDto[] = [];
  states: string[];
  types: string[];
  tags: string[];
  selectedFile: FileDto;
  selectedIndex: number;
  nameFormat = '第{0}话';
  getFilesDto: GetFilesDto;

  constructor(
    private animeServiceProxy: AnimeServiceProxy,
    private directoryServiceProxy: DirectoryServiceProxy
  ) {
    this.animeDetail.tags = [];
  }

  ngOnInit() {
    this.initData();
    this.getDetail();
    this.path = `${this.bangumi.name}/${this.anime.title}`;
  }

  initData(): void {
    const self = this;
    self.states = ['完结', '连载'];
    self.types = ['TV版', '剧场版', '真人版', 'OVA', 'OAD'];
    self.tags = [
      '原创', '漫改', '轻改', '游戏改', '动态漫', '布袋戏', '热血', '奇幻', '战斗', '搞笑', '日常', '科幻', '萌系', '治愈', '校园',
      '少儿', '泡面', '恋爱', '后宫', '猎奇', '少女', '魔法', '历史', '机战', '致郁', '神魔', '声控', '运动', '励志', '音乐', '推理',
      '社团', '智斗', '催泪', '美食', '装逼', '偶像', '乙女', '职场', '古风', '耽美', '情色'
    ];
  }

  getDetail(): void {
    const self = this;
    console.log(self.anime);
    self.animeServiceProxy.getdetail(self.anime.id).subscribe((rep) => {
      console.log(rep);
      self.animeDetail = rep;
      if (!self.animeDetail.tags) {
        self.animeDetail.tags = [];
      }
    });
  }

  getUrl(fileName: string): string {
    return `${this.path}/${fileName}`;
  }

  load(): void {
    const self = this;
    self.getFilesDto = new GetFilesDto();
    self.getFilesDto.path = self.drive + self.path;
    self.directoryServiceProxy.getFiles(self.getFilesDto).subscribe((rep) => {
      self.files = rep;
    });
  }

  renameDir(): void {
    const self = this;
    const input = new RenameDirectoryDto();
    input.sourcePath = self.getFilesDto.path;
    input.destPath = self.drive + self.path;
    self.directoryServiceProxy.renameDirectory(input).subscribe(() => {
      self.load();
    });
  }

  rename(): void {
    const self = this;
    const input = new RenameFilesDto();
    input.nameFormat = self.nameFormat;
    input.files = self.files;
    self.directoryServiceProxy.renameFiles(input).subscribe(success => {
      self.load();
    });
  }

  remove(file: any): void {
    const self = this;
    const i = self.files.indexOf(file);
    self.files.splice(i, 1);
  }

  highlight(index, file): void {
    this.selectedIndex = index;
    this.selectedFile = file;
  }

  up(): void {
    const self = this;
    if (self.selectedIndex === 0) {
      return;
    }
    self.swapItems(self.files, self.selectedIndex, self.selectedIndex - 1);
    self.selectedIndex = self.selectedIndex - 1;
  }

  down(): void {
    const self = this;
    if (self.selectedIndex === self.files.length - 1) {
      return;
    }
    self.swapItems(self.files, self.selectedIndex, self.selectedIndex + 1);
    self.selectedIndex = self.selectedIndex + 1;
  }

  selectTag(tag: string): void {
    const self = this;
    const index = self.animeDetail.tags.indexOf(tag);
    if (index > -1) {
      self.animeDetail.tags.splice(index, 1);
    } else {
      self.animeDetail.tags.push(tag);
    }
  }

  save(): void {
    const self = this;
    if (self.files && self.files.length > 0) {
      const resources = self.files.map((x) => {
        const resource = new Resource();
        resource.name = x.name.substring(0, x.name.lastIndexOf('.'));
        resource.url = self.getUrl(x.name);
        resource.metaData = new MediaMetaData();
        resource.metaData.duration = x.media.duration;
        resource.metaData.frameHeight = x.media.frameHeight;
        resource.metaData.frameRate = x.media.frameRate;
        resource.metaData.frameWidth = x.media.frameWidth;
        resource.metaData.length = x.length;
        return resource;
      });
      self.animeDetail.animes = resources;
    }
    console.log(self.animeDetail);
    self.animeServiceProxy.update(self.bangumi.id, self.anime).subscribe(() => {
      self.animeServiceProxy.updateDetail(self.animeDetail).subscribe(() => {
        self.getDetail();
        alert('update success.');
      });
    });
  }

  private swapItems(arr, index1, index2): void {
    arr[index1] = arr.splice(index2, 1, arr[index1])[0];
    return arr;
  }
}
