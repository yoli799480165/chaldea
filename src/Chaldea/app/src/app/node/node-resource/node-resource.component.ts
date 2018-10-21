import { Component, OnInit, Injector } from '@angular/core';
import { NodeServiceProxy, Node, GetDirFileDto, DirFileInfo, ExtractFileDto, SyncDirectory } from 'shared/service-proxies/service-proxies';
import { ComponentBase } from 'app/shared/component-base';
import { NodeBindingComponent } from '../node-binding/node-binding.component';
import { NodePublishComponent } from '../node-publish/node-publish.component';

@Component({
  selector: 'app-node-resource',
  templateUrl: './node-resource.component.html',
  styleUrls: ['./node-resource.component.scss']
})
export class NodeResourceComponent extends ComponentBase implements OnInit {
  selectedNode: Node;
  nodes: Node[];
  dirFiles: DirFileInfo[] = [];
  pathList: string[] = [];
  selectPath = '';
  selectDirFiles: DirFileInfo[] = [];
  syncDirs: SyncDirectory[] = [];

  constructor(
    private injector: Injector,
    private nodeServiceProxy: NodeServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getNodes();
  }

  getNodes(): void {
    this.loading.show();
    this.nodeServiceProxy.getNodes().subscribe((rep) => {
      this.nodes = rep;
      if (this.nodes && this.nodes.length > 0) {
        this.selectedNode = this.nodes[0];
        this.getSyncDirs();
      }
    });
  }

  getSyncDirs(): void {
    this.nodeServiceProxy.getSyncDirs(this.selectedNode.id).subscribe((rep) => {
      this.syncDirs = rep;
      this.getDirFiles();
    });
  }

  getDirFiles(): void {
    this.selectDirFiles = [];
    const input = new GetDirFileDto();
    input.path = this.selectPath;
    this.loading.show();
    this.nodeServiceProxy.getDirFiles(this.selectedNode.id, input).subscribe((rep) => {
      rep.forEach(dirFile => {
        dirFile['checked'] = false;
        dirFile['sync'] = false;
        dirFile['syncDir'] = '';
        this.syncDirs.forEach(syncDir => {
          if (dirFile.fullName === syncDir.local) {
            dirFile['sync'] = true;
            dirFile['syncDir'] = syncDir.remote;
            return false;
          }
        })
      });
      this.dirFiles = rep;
      this.loading.hide();
    }, (err) => {
      this.loading.hide();
    });
  }

  getSubDirFiles(dirFile: DirFileInfo): void {
    if (dirFile.type === 0) {
      this.setSelectPath(dirFile.fullName);
      this.getDirFiles();
    }
  }

  goHome(): void {
    this.setSelectPath('');
    this.getDirFiles();
  }

  goto(current: string): void {
    const index = this.pathList.indexOf(current) + 1;
    const fullPathList = this.pathList.slice(0, index);
    const fullPath = fullPathList.length === 1 ? `/${fullPathList[0]}/` : `/${fullPathList.join('/')}`;
    this.setSelectPath(fullPath);
    this.getDirFiles();
  }

  refresh(): void {
    this.getDirFiles();
  }

  setSelectPath(path: string): void {
    this.selectPath = path;
    if (this.selectPath === '') {
      this.pathList = [];
      return;
    }
    const replacePath = this.trim(path.replace(/\\/g, '>').replace(/\//g, '>'), '>');
    this.pathList = replacePath.split('>');
  }

  checkAll($event): void {
    this.selectDirFiles = [];
    this.dirFiles.forEach(item => {
      item['checked'] = $event.target.checked;
      if ($event.target.checked) {
        this.selectDirFiles.push(item);
      }
    });
  }

  checkItem($event, dirFile: DirFileInfo): void {
    if ($event.target.checked) {
      this.selectDirFiles.push(dirFile);
    } else {
      const index = this.selectDirFiles.indexOf(dirFile);
      this.selectDirFiles.splice(index, 1);
    }
  }

  deleteItems(): void {
    if (this.selectDirFiles.length > 0) {
      this.dialog.confirm('确定删除所选文件及文件夹?').subscribe((result) => {
        if (result) {
          this.nodeServiceProxy.deleteDirFiles(this.selectedNode.id, this.selectDirFiles).subscribe((msg) => {
            this.getDirFiles();
            if (msg && msg !== '') {
              this.dialog.alert(msg);
            }
          });
        }
      });
    }
  }

  extractFiles(): void {
    if (this.selectDirFiles.length > 0) {
      const extractParams = new ExtractFileDto();
      extractParams.files = this.selectDirFiles;
      extractParams.destDir = this.selectPath;
      extractParams.password = '';
      this.nodeServiceProxy.extractFiles(this.selectedNode.id, extractParams).subscribe((msg) => {
        this.getDirFiles();
        if (msg && msg !== '') {
          this.dialog.alert(msg);
        }
      });
    } else {
      this.dialog.alert('请选择压缩文件.');
    }
  }

  bindNetDisk(): void {
    if (this.selectDirFiles.length === 1) {
      this.modal.show(NodeBindingComponent, { nodeId: this.selectedNode.id, selectPath: this.selectDirFiles[0].fullName }).subscribe(() => {
        this.getSyncDirs();
      })
    } else if (this.selectDirFiles.length > 1) {
      this.dialog.alert('只能选择单个目录.');
    } else {
      this.dialog.alert('请选择目录.');
    }
  }

  sync(dirFile): void {
    this.dialog.confirm(`确定要同步文件夹:${dirFile.syncDir}`).subscribe((result) => {
      if (result) {
        const input = new SyncDirectory();
        input.local = dirFile.fullName;
        input.remote = dirFile.syncDir;
        this.nodeServiceProxy.syncDir(this.selectedNode.id, input).subscribe((msg) => {
          if (msg && msg !== '') {
            this.dialog.alert(msg);
          }
        });
      }
    });
  }

  publish(): void {
    if (this.selectDirFiles.length > 0) {
      const data = {
        nodeId: this.selectedNode.id,
        selectPath: this.selectPath,
        selectDirFiles: this.selectDirFiles
      };
      this.modal.show(NodePublishComponent, data, { class: 'modal-lg' }).subscribe((rep) => {
        this.showTips('发布成功', 'success');
      });
    } else {
      this.dialog.alert('请选择要发布的文件.');
    }
  }

  trim(s: string, c: string): string {
    if (c === ']') {
      c = '\\]';
    }
    if (c === '\\') {
      c = '\\\\';
    }
    return s.replace(new RegExp(
      '^[' + c + ']+|[' + c + ']+$', 'g'
    ), '');
  }
}
