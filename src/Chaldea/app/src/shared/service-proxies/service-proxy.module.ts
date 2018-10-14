import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';

@NgModule({
  providers: [
    ApiServiceProxies.AnimeServiceProxy,
    ApiServiceProxies.AnimeTagServiceProxy,
    ApiServiceProxies.BangumiServiceProxy,
  ],
})
export class ServiceProxyModule {
}
