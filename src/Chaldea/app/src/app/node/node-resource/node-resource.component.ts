import { Component, OnInit, Injector } from '@angular/core';
import { NodeServiceProxy, Node, GetDirFileDto, DirFileInfo, ExtractFileDto } from 'shared/service-proxies/service-proxies';
import { ComponentBase } from 'app/shared/component-base';

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
    this.nodeServiceProxy.getNodes().subscribe((rep) => {
      this.nodes = rep;
      if (this.nodes && this.nodes.length > 0) {
        this.selectedNode = this.nodes[0];
        this.getDirFiles();
      }
    });
  }

  getDirFiles(): void {
    this.selectDirFiles = [];
    const input = new GetDirFileDto();
    input.path = this.selectPath;
    console.log(this.pathList);
    this.nodeServiceProxy.getDirFiles(this.selectedNode.id, input).subscribe((rep) => {
      this.dirFiles = rep;
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
    const fullPath = fullPathList.length === 1 ? `${fullPathList[0]}/` : fullPathList.join('/');
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
      this.dialog.confirm('确定删除所选文件及文件夹?').subscribe((rep) => {
        if (rep) {
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
    }
  }

  fileSize(length: number): string {
    const units = ['B', 'KB', 'MB', 'GB', 'TB', 'PB'];
    const mod = 1024.0;
    let i = 0;
    while (length >= mod) {
      length /= mod;
      i++;
    }
    return Math.round(length) + units[i];
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
