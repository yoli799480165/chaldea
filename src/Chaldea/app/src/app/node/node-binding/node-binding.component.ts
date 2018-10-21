import { Component, OnInit, Inject, Injector } from '@angular/core';
import { TreeModel, Tree } from 'ng2-tree';
import { ComponentBase } from 'app/shared/component-base';
import { NodeServiceProxy, GetDirFileDto, DirFileInfoType, SyncDirectory } from 'shared/service-proxies/service-proxies';

@Component({
  selector: 'app-node-binding',
  templateUrl: './node-binding.component.html',
  styleUrls: ['./node-binding.component.scss']
})
export class NodeBindingComponent extends ComponentBase implements OnInit {
  rootName = '我的网盘';
  tree: TreeModel = {
    value: this.rootName,
    loadChildren: (callback) => {
      this.getDirFiles(callback);
    },
    settings: {
      rightMenu: false,
      leftMenu: false,
      templates: {
        'node': '<i class="fa fa-folder-o fa-lg"></i>',
        'leaf': '<i class="fa fa-file-o fa-lg"></i>'
      }
    },
  };
  localPath: string;
  nodeId: string;
  remotePath = '';
  remoteSelectPath = '';

  constructor(
    private injector: Injector,
    private nodeServiceProxy: NodeServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    setTimeout(() => {
      if (this.modal.data) {
        this.nodeId = this.modal.data.nodeId;
        this.localPath = this.modal.data.selectPath;
      }
    }, 0);
  }

  getDirFiles(callback: (children: TreeModel[]) => void): void {
    const self = this;
    const input = new GetDirFileDto();
    input.path = self.remotePath;
    self.nodeServiceProxy.getNetDiskDirFiles(self.nodeId, input).subscribe((rep) => {
      const items = rep.filter(x => x.type === DirFileInfoType._0).map(x => {
        const item = <TreeModel>{};
        item.value = x.name;
        item.id = x.fullName;
        item.loadChildren = (itemcallback) => {
          self.getDirFiles(itemcallback);
        };
        return item;
      });
      console.log(items);
      callback(items);
    });
  }

  bind(): void {
    const input = new SyncDirectory();
    input.local = this.localPath;
    input.remote = this.remoteSelectPath;
    this.nodeServiceProxy.bindSyncDir(this.nodeId, input).subscribe(() => {
      this.modal.dismiss(true);
    });
  }

  handleExpanded($event): void {
    if ($event.node.value !== this.rootName) {
      this.remotePath = <string>$event.node.id;
    }
  }

  handleSelected($event): void {
    if ($event.node.id) {
      this.remoteSelectPath = $event.node.id;
    }
  }
}
