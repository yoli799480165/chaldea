import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  userName: string;
  password: string;

  constructor(
    private loginService: LoginService
  ) { }

  ngOnInit() {
  }

  login(): void {
    this.loginService.login(this.userName, this.password);
  }
}
