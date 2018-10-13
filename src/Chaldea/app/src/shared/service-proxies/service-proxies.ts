﻿/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v11.12.7.0 (NJsonSchema v9.10.6.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';

import * as moment from 'moment';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class BangumiServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @input (optional) 
     * @return Success
     */
    createOrUpdate(input: BangumiEditDto | null): Observable<void> {
        let url_ = this.baseUrl + "/api/bangumi/createOrUpdate";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request("put", url_, options_).flatMap((response_ : any) => {
            return this.processCreateOrUpdate(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processCreateOrUpdate(response_);
                } catch (e) {
                    return <Observable<void>><any>Observable.throw(e);
                }
            } else
                return <Observable<void>><any>Observable.throw(response_);
        });
    }

    protected processCreateOrUpdate(response: HttpResponse<Blob>): Observable<void> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            return Observable.of<void>(<any>null);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<void>(<any>null);
    }

    /**
     * @return Success
     */
    delete(id: string): Observable<void> {
        let url_ = this.baseUrl + "/api/bangumi/{id}/delete";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id)); 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request("delete", url_, options_).flatMap((response_ : any) => {
            return this.processDelete(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processDelete(response_);
                } catch (e) {
                    return <Observable<void>><any>Observable.throw(e);
                }
            } else
                return <Observable<void>><any>Observable.throw(response_);
        });
    }

    protected processDelete(response: HttpResponse<Blob>): Observable<void> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            return Observable.of<void>(<any>null);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<void>(<any>null);
    }

    /**
     * @return Success
     */
    getList(): Observable<BangumiDto[]> {
        let url_ = this.baseUrl + "/api/bangumi/getList";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).flatMap((response_ : any) => {
            return this.processGetList(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processGetList(response_);
                } catch (e) {
                    return <Observable<BangumiDto[]>><any>Observable.throw(e);
                }
            } else
                return <Observable<BangumiDto[]>><any>Observable.throw(response_);
        });
    }

    protected processGetList(response: HttpResponse<Blob>): Observable<BangumiDto[]> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (resultData200 && resultData200.constructor === Array) {
                result200 = [];
                for (let item of resultData200)
                    result200.push(BangumiDto.fromJS(item));
            }
            return Observable.of(result200);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<BangumiDto[]>(<any>null);
    }

    /**
     * @input (optional) 
     * @return Success
     */
    import(id: string, input: ImportBangumiDto | null): Observable<void> {
        let url_ = this.baseUrl + "/api/bangumi/{id}/import";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id)); 
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request("post", url_, options_).flatMap((response_ : any) => {
            return this.processImport(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processImport(response_);
                } catch (e) {
                    return <Observable<void>><any>Observable.throw(e);
                }
            } else
                return <Observable<void>><any>Observable.throw(response_);
        });
    }

    protected processImport(response: HttpResponse<Blob>): Observable<void> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            return Observable.of<void>(<any>null);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<void>(<any>null);
    }
}

@Injectable()
export class BannerServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @banner (optional) 
     * @return Success
     */
    add(banner: Banner | null): Observable<void> {
        let url_ = this.baseUrl + "/api/banner/add";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(banner);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request("put", url_, options_).flatMap((response_ : any) => {
            return this.processAdd(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processAdd(response_);
                } catch (e) {
                    return <Observable<void>><any>Observable.throw(e);
                }
            } else
                return <Observable<void>><any>Observable.throw(response_);
        });
    }

    protected processAdd(response: HttpResponse<Blob>): Observable<void> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            return Observable.of<void>(<any>null);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<void>(<any>null);
    }

    /**
     * @skip (optional) 
     * @take (optional) 
     * @return Success
     */
    getList(skip: number | null, take: number | null): Observable<Banner[]> {
        let url_ = this.baseUrl + "/api/banner/getList?";
        if (skip !== undefined)
            url_ += "skip=" + encodeURIComponent("" + skip) + "&"; 
        if (take !== undefined)
            url_ += "take=" + encodeURIComponent("" + take) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).flatMap((response_ : any) => {
            return this.processGetList(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processGetList(response_);
                } catch (e) {
                    return <Observable<Banner[]>><any>Observable.throw(e);
                }
            } else
                return <Observable<Banner[]>><any>Observable.throw(response_);
        });
    }

    protected processGetList(response: HttpResponse<Blob>): Observable<Banner[]> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (resultData200 && resultData200.constructor === Array) {
                result200 = [];
                for (let item of resultData200)
                    result200.push(Banner.fromJS(item));
            }
            return Observable.of(result200);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<Banner[]>(<any>null);
    }
}

@Injectable()
export class UserServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @return Success
     */
    getUsers(): Observable<void> {
        let url_ = this.baseUrl + "/api/user/getUsers";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request("get", url_, options_).flatMap((response_ : any) => {
            return this.processGetUsers(response_);
        }).catch((response_: any) => {
            if (response_ instanceof HttpResponse) {
                try {
                    return this.processGetUsers(response_);
                } catch (e) {
                    return <Observable<void>><any>Observable.throw(e);
                }
            } else
                return <Observable<void>><any>Observable.throw(response_);
        });
    }

    protected processGetUsers(response: HttpResponse<Blob>): Observable<void> {
        const status = response.status; 

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(response.body).flatMap(_responseText => {
            return Observable.of<void>(<any>null);
            });
        } else if (status !== 200 && status !== 204) {
            return blobToText(response.body).flatMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Observable.of<void>(<any>null);
    }
}

export class BangumiEditDto implements IBangumiEditDto {
    id: string | undefined;
    name: string | undefined;

    constructor(data?: IBangumiEditDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.name = data["name"];
        }
    }

    static fromJS(data: any): BangumiEditDto {
        let result = new BangumiEditDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        return data; 
    }
}

export interface IBangumiEditDto {
    id: string | undefined;
    name: string | undefined;
}

export class BangumiDto implements IBangumiDto {
    id: string | undefined;
    name: string | undefined;

    constructor(data?: IBangumiDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.name = data["name"];
        }
    }

    static fromJS(data: any): BangumiDto {
        let result = new BangumiDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        return data; 
    }
}

export interface IBangumiDto {
    id: string | undefined;
    name: string | undefined;
}

export class ImportBangumiDto implements IImportBangumiDto {
    clear: boolean | undefined;
    url: string | undefined;

    constructor(data?: IImportBangumiDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.clear = data["clear"];
            this.url = data["url"];
        }
    }

    static fromJS(data: any): ImportBangumiDto {
        let result = new ImportBangumiDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["clear"] = this.clear;
        data["url"] = this.url;
        return data; 
    }
}

export interface IImportBangumiDto {
    clear: boolean | undefined;
    url: string | undefined;
}

export class Banner implements IBanner {
    image: string | undefined;
    link: string | undefined;
    title: string | undefined;
    id: string | undefined;

    constructor(data?: IBanner) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.image = data["image"];
            this.link = data["link"];
            this.title = data["title"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): Banner {
        let result = new Banner();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["image"] = this.image;
        data["link"] = this.link;
        data["title"] = this.title;
        data["id"] = this.id;
        return data; 
    }
}

export interface IBanner {
    image: string | undefined;
    link: string | undefined;
    title: string | undefined;
    id: string | undefined;
}

export class SwaggerException extends Error {
    message: string;
    status: number; 
    response: string; 
    headers: { [key: string]: any; };
    result: any; 

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if(result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader(); 
            reader.onload = function() { 
                observer.next(this.result);
                observer.complete();
            }
            reader.readAsText(blob); 
        }
    });
}