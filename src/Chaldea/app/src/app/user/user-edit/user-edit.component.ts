import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from 'app/shared/component-base';
import { UserServiceProxy, UserDto, DropdownItem } from 'shared/service-proxies/service-proxies';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent extends ComponentBase implements OnInit {
  input: UserDto = new UserDto();
  roles: DropdownItem[] = [];
  selectRole: DropdownItem;

  constructor(
    private injector: Injector,
    private userServiceProxy: UserServiceProxy
  ) {
    super(injector);
    this.input.isActive = true;
  }

  ngOnInit() {
    this.getRoles();
  }

  getRoles(): void {
    this.userServiceProxy.getRoles().subscribe((rep) => {
      this.roles = rep;
      this.selectRole = rep[0];
    });
  }

  save(): void {
    this.loading.show();
    this.input.role = this.selectRole.value;
    this.userServiceProxy.addUser(this.input).subscribe(() => {
      this.loading.hide();
      this.modal.dismiss(true);
    });
  }
}
