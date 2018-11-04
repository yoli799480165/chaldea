import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AppConsts } from 'shared/AppConsts';
import { TokenService, TokenDto } from 'app/shared/token.service';

@Injectable()
export class LoginService {
    constructor(
        private router: Router,
        private httpClient: HttpClient,
        private tokenService: TokenService
    ) {

    }

    login(userName: string, password: string): void {
        const body = new FormData();
        body.append('username', userName);
        body.append('password', password);
        body.append('grant_type', 'password');
        body.append('client_id', 'chaldea-mobile');
        body.append('client_secret', '613b807c57a641e881be80c6333de409');
        this.httpClient.post(`${AppConsts.idServerUrl}/connect/token`, body)
            .subscribe((rep) => {
                this.tokenService.setToken(<TokenDto>rep);
                this.router.navigate(['/app/dashboard']);
            }, () => {
            });
    }
}
