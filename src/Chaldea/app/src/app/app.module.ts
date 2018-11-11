import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { SortablejsModule } from 'angular-sortablejs';
import { ModalService } from 'app/shared/modal-service';
import { DictionaryPipe } from 'app/shared/pipes/dictionary.pipe';
import { SafeHtmlPipe } from 'app/shared/pipes/safe-html.pipe';
import { TreeModule } from 'ng2-tree';
import { BsDatepickerModule, ModalModule, TimepickerModule } from 'ngx-bootstrap';
import { ClipboardModule } from 'ngx-clipboard';
import { NgxCoolDialogsModule } from 'ngx-cool-dialogs';
import { NgxLoadingModule } from 'ngx-loading';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { AppConsts } from 'shared/AppConsts';
import { ServiceProxyModule } from 'shared/service-proxies/service-proxy.module';

import { API_BASE_URL } from '../shared/service-proxies/service-proxies';
import { LoginComponent } from './account/login/login.component';
import { AnimeDetailComponent } from './anime/anime-detail/anime-detail.component';
import { AnimeImportComponent } from './anime/anime-import/anime-import.component';
import { AnimeComponent } from './anime/anime.component';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing';
import { BangumiEditComponent } from './bangumi/bangumi-edit/bangumi-edit.component';
import { BangumiComponent } from './bangumi/bangumi.component';
import { HomeComponent } from './home/home.component';
import { IconsComponent } from './icons/icons.component';
import { LbdModule } from './lbd/lbd.module';
import { NodeBindingComponent } from './node/node-binding/node-binding.component';
import { NodePublishComponent } from './node/node-publish/node-publish.component';
import { ExtractFileComponent } from './node/node-resource/extract-file/extract-file.component';
import { NodeResourceComponent } from './node/node-resource/node-resource.component';
import { NodeComponent } from './node/node.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { FooterModule } from './shared/footer/footer.module';
import { RefreshTokenHttpInterceptor } from './shared/http-interceptor';
import { LayoutComponent } from './shared/layout/layout.component';
import { LoadingService } from './shared/loading-service';
import { NavbarModule } from './shared/navbar/navbar.module';
import { RouterService } from './shared/router-service';
import { SidebarModule } from './sidebar/sidebar.module';
import { TablesComponent } from './tables/tables.component';
import { TimetableEditComponent } from './timetable/timetable-edit/timetable-edit.component';
import { TimetableComponent } from './timetable/timetable.component';
import { TypographyComponent } from './typography/typography.component';
import { UpgradeComponent } from './upgrade/upgrade.component';
import { UserComponent } from './user/user.component';
import { LoginService } from './account/login/login.service';
import { TokenService } from './shared/token.service';
import { UserEditComponent } from './user/user-edit/user-edit.component';

export function getRemoteServiceBaseUrl(): string {
  return AppConsts.appBaseUrl;
}

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
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
    AnimeDetailComponent,
    NodeComponent,
    NodeResourceComponent,
    NodeBindingComponent,
    NodePublishComponent,
    ExtractFileComponent,
    LoginComponent,
    TimetableComponent,
    TimetableEditComponent,
    UserEditComponent
  ],
  imports: [
    NgxCoolDialogsModule.forRoot(),
    ModalModule.forRoot(),
    SortablejsModule.forRoot({ animation: 150 }),
    NgxLoadingModule.forRoot({}),
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
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
    ServiceProxyModule,
    TreeModule,
    SelectDropDownModule,
    ClipboardModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: RefreshTokenHttpInterceptor, multi: true },
    { provide: API_BASE_URL, useFactory: getRemoteServiceBaseUrl },
    TokenService,
    LoginService,
    ModalService,
    LoadingService,
    RouterService
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    BangumiEditComponent,
    AnimeImportComponent,
    NodeBindingComponent,
    NodePublishComponent,
    ExtractFileComponent,
    TimetableEditComponent,
    UserEditComponent
  ]
})
export class AppModule { }
