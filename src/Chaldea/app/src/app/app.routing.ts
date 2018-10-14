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

const routes: Routes = [
  { path: 'dashboard', component: HomeComponent },
  { path: 'bangumi', component: BangumiComponent },
  { path: 'anime', component: AnimeComponent },
  { path: 'anime-detail/:animeId', component: AnimeDetailComponent },
  { path: 'notifications', component: NotificationsComponent },
  { path: 'upgrade', component: UpgradeComponent },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
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
