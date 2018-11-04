import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable()
export class ChaldeaHttpInterceptor implements HttpInterceptor {
    private _tokenService = new TokenService();

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const modifiedRequest = this.normalizeRequestHeaders(request);
        return next.handle(modifiedRequest);
    }

    protected normalizeRequestHeaders(request: HttpRequest<any>): HttpRequest<any> {
        let modifiedHeaders = new HttpHeaders();
        modifiedHeaders = request.headers
            .set('Pragma', 'no-cache')
            .set('Cache-Control', 'no-cache')
            .set('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT');

        modifiedHeaders = this.addXRequestedWithHeader(modifiedHeaders);
        modifiedHeaders = this.addAuthorizationHeaders(modifiedHeaders);

        return request.clone({
            headers: modifiedHeaders
        });
    }

    protected addXRequestedWithHeader(headers: HttpHeaders): HttpHeaders {
        if (headers) {
            headers = headers.set('X-Requested-With', 'XMLHttpRequest');
        }

        return headers;
    }

    protected addAuthorizationHeaders(headers: HttpHeaders): HttpHeaders {
        const token = this._tokenService.getToken();
        if (headers && token) {
            headers = headers.set('Authorization', `Bearer ${token}`);
        }

        return headers;
    }
}
