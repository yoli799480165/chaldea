﻿/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v11.18.7.0 (NJsonSchema v9.10.70.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, from as _observableFrom, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

import * as moment from 'moment';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class DirectoryServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    getFiles(input: GetFilesDto | null | undefined): Observable<FileDto[]> {
        let url_ = this.baseUrl + "/api/chaldea/directory/getFiles";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetFiles(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetFiles(<any>response_);
                } catch (e) {
                    return <Observable<FileDto[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto[]>><any>_observableThrow(response_);
        }));
    }

    protected processGetFiles(response: HttpResponseBase): Observable<FileDto[]> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (resultData200 && resultData200.constructor === Array) {
                result200 = [];
                for (let item of resultData200)
                    result200.push(FileDto.fromJS(item));
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileDto[]>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    renameFiles(input: RenameFilesDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/chaldea/directory/renameFiles";
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

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processRenameFiles(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processRenameFiles(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processRenameFiles(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    renameDirectory(input: RenameDirectoryDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/chaldea/directory/renameDirectory";
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

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processRenameDirectory(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processRenameDirectory(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processRenameDirectory(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    searchFiles(input: SearchFilesDto | null | undefined): Observable<FileDto[]> {
        let url_ = this.baseUrl + "/api/chaldea/directory/searchFiles";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processSearchFiles(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSearchFiles(<any>response_);
                } catch (e) {
                    return <Observable<FileDto[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto[]>><any>_observableThrow(response_);
        }));
    }

    protected processSearchFiles(response: HttpResponseBase): Observable<FileDto[]> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (resultData200 && resultData200.constructor === Array) {
                result200 = [];
                for (let item of resultData200)
                    result200.push(FileDto.fromJS(item));
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileDto[]>(<any>null);
    }
}

export class GetFilesDto implements IGetFilesDto {
    path!: string | undefined;

    constructor(data?: IGetFilesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.path = data["path"];
        }
    }

    static fromJS(data: any): GetFilesDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetFilesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["path"] = this.path;
        return data; 
    }
}

export interface IGetFilesDto {
    path: string | undefined;
}

export class FileDto implements IFileDto {
    name!: string | undefined;
    fullName!: string | undefined;
    directoryName!: string | undefined;
    size!: string | undefined;
    length!: number | undefined;
    media!: MediaInfo | undefined;

    constructor(data?: IFileDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.name = data["name"];
            this.fullName = data["fullName"];
            this.directoryName = data["directoryName"];
            this.size = data["size"];
            this.length = data["length"];
            this.media = data["media"] ? MediaInfo.fromJS(data["media"]) : <any>undefined;
        }
    }

    static fromJS(data: any): FileDto {
        data = typeof data === 'object' ? data : {};
        let result = new FileDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["fullName"] = this.fullName;
        data["directoryName"] = this.directoryName;
        data["size"] = this.size;
        data["length"] = this.length;
        data["media"] = this.media ? this.media.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IFileDto {
    name: string | undefined;
    fullName: string | undefined;
    directoryName: string | undefined;
    size: string | undefined;
    length: number | undefined;
    media: MediaInfo | undefined;
}

export class MediaInfo implements IMediaInfo {
    duration!: number | undefined;
    frameWidth!: number | undefined;
    frameHeight!: number | undefined;
    frameRate!: number | undefined;

    constructor(data?: IMediaInfo) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.duration = data["duration"];
            this.frameWidth = data["frameWidth"];
            this.frameHeight = data["frameHeight"];
            this.frameRate = data["frameRate"];
        }
    }

    static fromJS(data: any): MediaInfo {
        data = typeof data === 'object' ? data : {};
        let result = new MediaInfo();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["duration"] = this.duration;
        data["frameWidth"] = this.frameWidth;
        data["frameHeight"] = this.frameHeight;
        data["frameRate"] = this.frameRate;
        return data; 
    }
}

export interface IMediaInfo {
    duration: number | undefined;
    frameWidth: number | undefined;
    frameHeight: number | undefined;
    frameRate: number | undefined;
}

export class RenameFilesDto implements IRenameFilesDto {
    nameFormat!: string | undefined;
    files!: FileDto[] | undefined;

    constructor(data?: IRenameFilesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.nameFormat = data["nameFormat"];
            if (data["files"] && data["files"].constructor === Array) {
                this.files = [];
                for (let item of data["files"])
                    this.files.push(FileDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): RenameFilesDto {
        data = typeof data === 'object' ? data : {};
        let result = new RenameFilesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["nameFormat"] = this.nameFormat;
        if (this.files && this.files.constructor === Array) {
            data["files"] = [];
            for (let item of this.files)
                data["files"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IRenameFilesDto {
    nameFormat: string | undefined;
    files: FileDto[] | undefined;
}

export class RenameDirectoryDto implements IRenameDirectoryDto {
    sourcePath!: string | undefined;
    destPath!: string | undefined;

    constructor(data?: IRenameDirectoryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.sourcePath = data["sourcePath"];
            this.destPath = data["destPath"];
        }
    }

    static fromJS(data: any): RenameDirectoryDto {
        data = typeof data === 'object' ? data : {};
        let result = new RenameDirectoryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["sourcePath"] = this.sourcePath;
        data["destPath"] = this.destPath;
        return data; 
    }
}

export interface IRenameDirectoryDto {
    sourcePath: string | undefined;
    destPath: string | undefined;
}

export class SearchFilesDto implements ISearchFilesDto {
    path!: string | undefined;
    searchPattern!: string | undefined;

    constructor(data?: ISearchFilesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.path = data["path"];
            this.searchPattern = data["searchPattern"];
        }
    }

    static fromJS(data: any): SearchFilesDto {
        data = typeof data === 'object' ? data : {};
        let result = new SearchFilesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["path"] = this.path;
        data["searchPattern"] = this.searchPattern;
        return data; 
    }
}

export interface ISearchFilesDto {
    path: string | undefined;
    searchPattern: string | undefined;
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
        return _observableThrow(result);
    else
        return _observableThrow(new SwaggerException(message, status, response, headers, null));
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