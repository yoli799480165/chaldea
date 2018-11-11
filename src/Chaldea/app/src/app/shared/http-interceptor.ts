import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { TokenService, TokenDto } from './token.service';
import { AppConsts } from 'shared/AppConsts';

@Injectable()
export class RefreshTokenHttpInterceptor implements HttpInterceptor {
    private _tokenService = new TokenService();

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const self = this;
        const subject = new Subject<HttpEvent<any>>();
        const modifiedRequest = self.normalizeRequestHeaders(request);
        next.handle(modifiedRequest).subscribe((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {
                subject.next(event);
                subject.complete();
            }
        }, (error: HttpErrorResponse) => {
            if (error.status === 401) {
                self.refreshToken(request, next).subscribe(() => {
                    const newRequest = self.normalizeRequestHeaders(request);
                    next.handle(newRequest).subscribe((event: HttpEvent<any>) => {
                        if (event instanceof HttpResponse) {
                            subject.next(event);
                            subject.complete();
                        }
                    }, (reqError) => {
                        subject.error(reqError);
                        subject.complete();
                    });
                }, refreshError => {
                    subject.error(refreshError);
                    subject.complete();
                });
            } else {
                subject.error(error);
                subject.complete();
            }
        });
        return subject;
    }

    protected normalizeRequestHeaders(request: HttpRequest<any>): HttpRequest<any> {
        let modifiedHeaders = new HttpHeaders();
        modifiedHeaders = request.headers
            .set('Pragma', 'no-cache')
            .set('Cache-Control', 'no-cache')
            .set('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT')
            .set('X-Requested-With', 'XMLHttpRequest');

        const token = this._tokenService.getToken();
        if (token) {
            modifiedHeaders = modifiedHeaders.set('Authorization', `Bearer ${token}`);
        }

        return request.clone({
            headers: modifiedHeaders
        });
    }

    private refreshToken(request: HttpRequest<any>, next: HttpHandler): Subject<HttpEvent<any>> {
        const self = this;
        const subject = new Subject<HttpEvent<any>>();
        let headers = new HttpHeaders();
        headers = request.headers.set('Content-Type', 'application/x-www-form-urlencoded');
        const body = new URLSearchParams();
        body.set('refresh_token', self._tokenService.getRefreshToken());
        body.set('grant_type', 'refresh_token');
        body.set('client_id', AppConsts.clientId);
        body.set('client_secret', AppConsts.clientSecret);
        const tokenRequest = request.clone({
            method: 'POST',
            url: `${AppConsts.idServerUrl}/connect/token`,
            headers: headers,
            body: body.toString()
        });
        next.handle(tokenRequest).subscribe((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {
                self.blobToText(event.body).subscribe(json => {
                    const token = <TokenDto>JSON.parse(json);
                    self._tokenService.setToken(token);
                    subject.next(event);
                    subject.complete();
                });
            }
        }, error => {
            subject.error(error);
            subject.complete();
        });
        return subject;
    }

    private blobToText(blob: any): Observable<string> {
        return new Observable<string>((observer: any) => {
            if (!blob) {
                observer.next('');
                observer.complete();
            } else {
                const reader = new FileReader();
                reader.onload = function () {
                    observer.next(this.result);
                    observer.complete();
                }
                reader.readAsText(blob);
            }
        });
    }
}
