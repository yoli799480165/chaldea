import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { LayoutComponent } from './layout/layout.component';
import { AppRoutingModule } from './app.routing';
import { BangumiComponent } from './bangumi/bangumi.component';
import { ServiceProxyModule } from '../shared/service-proxy.module';
import { AppConsts } from '../shared/AppConsts';
import { CHALDEA_BASE_URL } from '../shared/chaldea/chaldea-proxies';
import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { HttpClientModule } from '@angular/common/http';
import { BangumiListComponent } from './bangumi/bangumi-list/bangumi-list.component';
import { BangumiAnimeComponent } from './bangumi/bangumi-anime/bangumi-anime.component';
import { BangumiAnimeDetailComponent } from './bangumi/bangumi-anime-detail/bangumi-anime-detail.component';
import { FormsModule } from '@angular/forms';

export function getChaldeaServiceBaseUrl(): string {
  return AppConsts.chaldeaBaseUrl;
}

export function getApiServiceBaseUrl(): string {
  return AppConsts.apiBaseUrl;
}

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    BangumiComponent,
    BangumiListComponent,
    BangumiAnimeComponent,
    BangumiAnimeDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    ServiceProxyModule
  ],
  providers: [
    { provide: CHALDEA_BASE_URL, useFactory: getChaldeaServiceBaseUrl },
    { provide: API_BASE_URL, useFactory: getApiServiceBaseUrl },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
