import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { UpgradeComponent } from './upgrade/upgrade.component';
import { BangumiComponent } from './bangumi/bangumi.component';
import { AnimeComponent } from './anime/anime.component';
import { AnimeDetailComponent } from './anime/anime-detail/anime-detail.component';
import { NodeResourceComponent } from './node/node-resource/node-resource.component';
import { NodePublishComponent } from './node/node-publish/node-publish.component';
import { LoginComponent } from './account/login/login.component';
import { LayoutComponent } from './shared/layout/layout.component';
import { TimetableComponent } from './timetable/timetable.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'app',
    component: LayoutComponent,
    children: [
      { path: 'dashboard', component: HomeComponent },
      { path: 'bangumi', component: BangumiComponent },
      { path: 'anime', component: AnimeComponent },
      { path: 'anime/:bangumi', component: AnimeComponent },
      { path: 'anime-detail/:animeId', component: AnimeDetailComponent },
      { path: 'node-resource', component: NodeResourceComponent },
      { path: 'node-publish/:key', component: NodePublishComponent },
      { path: 'timetable', component: TimetableComponent },
      { path: 'user', component: UserComponent },
      { path: 'upgrade', component: UpgradeComponent }
    ]
  },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
  ],
})
export class AppRoutingModule { }
