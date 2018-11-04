import { Injectable } from '@angular/core';

export class TokenDto {
    access_token: string;
    refresh_token: string;
    token_type: string;
    expires_in: number;
}

@Injectable()
export class TokenService {
    public setToken(token: TokenDto): void {
        localStorage.setItem('access_token', token.access_token);
        localStorage.setItem('refresh_token', token.refresh_token);
    }

    public getToken() {
        return localStorage.getItem('access_token');
    }

    public getRefreshToken() {
        return localStorage.getItem('refresh_token');
    }
}
