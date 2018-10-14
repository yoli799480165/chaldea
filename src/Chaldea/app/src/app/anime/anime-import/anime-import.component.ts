import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '../../shared/component-base';
import { BangumiServiceProxy, ImportBangumiDto } from '../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'app-anime-import',
  templateUrl: './anime-import.component.html',
  styleUrls: ['./anime-import.component.scss']
})
export class AnimeImportComponent extends ComponentBase implements OnInit {
  input: ImportBangumiDto = new ImportBangumiDto();
  bangumiId: string;

  constructor(
    private injector: Injector,
    private bangumiServiceProxy: BangumiServiceProxy
  ) {
    super(injector);
    this.input.clear = false;
  }

  ngOnInit() {
    setTimeout(() => {
      if (this.modal.data) {
        this.bangumiId = this.modal.data;
      }
    }, 0);
  }

  import(): void {
    this.bangumiServiceProxy.import(this.bangumiId, this.input).subscribe(() => {
      this.modal.dismiss(true);
    });
  }
}
