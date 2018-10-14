import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { DictionaryPipe } from 'app/shared/pipes/dictionary.pipe';
import { SafeHtmlPipe } from 'app/shared/pipes/safe-html.pipe';
import { NgxCoolDialogsModule } from 'ngx-cool-dialogs';
import { AppConsts } from 'shared/AppConsts';
import { ServiceProxyModule } from 'shared/service-proxies/service-proxy.module';

import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing';
import { HomeComponent } from './home/home.component';
import { IconsComponent } from './icons/icons.component';
import { LbdModule } from './lbd/lbd.module';
import { NotificationsComponent } from './notifications/notifications.component';
import { FooterModule } from './shared/footer/footer.module';
import { NavbarModule } from './shared/navbar/navbar.module';
import { SidebarModule } from './sidebar/sidebar.module';
import { TablesComponent } from './tables/tables.component';
import { TypographyComponent } from './typography/typography.component';
import { UpgradeComponent } from './upgrade/upgrade.component';
import { UserComponent } from './user/user.component';
import { ModalService } from 'app/shared/modal-service';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BangumiComponent } from './bangumi/bangumi.component';
import { BangumiEditComponent } from './bangumi/bangumi-edit/bangumi-edit.component';
import { AnimeComponent } from './anime/anime.component';
import { AnimeImportComponent } from './anime/anime-import/anime-import.component';
import { AnimeDetailComponent } from './anime/anime-detail/anime-detail.component';

export function getRemoteServiceBaseUrl(): string {
  return AppConsts.appBaseUrl;
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserComponent,
    TablesComponent,
    TypographyComponent,
    IconsComponent,
    NotificationsComponent,
    UpgradeComponent,
    SafeHtmlPipe,
    DictionaryPipe,
    BangumiComponent,
    BangumiEditComponent,
    AnimeComponent,
    AnimeImportComponent,
    AnimeDetailComponent
  ],
  imports: [
    NgxCoolDialogsModule.forRoot(),
    ModalModule.forRoot(),
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    NavbarModule,
    FooterModule,
    SidebarModule,
    RouterModule,
    AppRoutingModule,
    LbdModule,
    HttpClientModule,
    ServiceProxyModule
  ],
  providers: [
    ModalService,
    { provide: API_BASE_URL, useFactory: getRemoteServiceBaseUrl }
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    BangumiEditComponent,
    AnimeImportComponent
  ]
})
export class AppModule { }
