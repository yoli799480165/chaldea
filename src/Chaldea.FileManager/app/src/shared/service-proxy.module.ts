import { NgModule } from '@angular/core';

import * as ChaldeaServiceProxies from './chaldea/chaldea-proxies';
import * as ApiServiceProxies from './service-proxies/service-proxies';


@NgModule({
  providers: [
    ChaldeaServiceProxies.BangumiServiceProxy,
    ChaldeaServiceProxies.AnimeServiceProxy,
    ChaldeaServiceProxies.BannerServiceProxy,

    ApiServiceProxies.DirectoryServiceProxy
  ]
})
export class ServiceProxyModule { }
