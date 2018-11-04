import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from 'app/shared/component-base';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserServiceProxy, UserDto } from 'shared/service-proxies/service-proxies';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent extends ComponentBase implements OnInit {
  users: UserDto[] = [];

  constructor(
    private injector: Injector,
    private userServiceProxy: UserServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getList();
  }

  getList(): void {
    this.userServiceProxy.getUsers(0, 0).subscribe((rep) => {
      this.users = rep;
    });
  }

  add(): void {
    this.modal.show(UserEditComponent).subscribe(() => {
      this.getList();
    });
  }
}
