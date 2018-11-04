import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';

@NgModule({
  providers: [
    ApiServiceProxies.AnimeServiceProxy,
    ApiServiceProxies.AnimeTagServiceProxy,
    ApiServiceProxies.BangumiServiceProxy,
    ApiServiceProxies.NodeServiceProxy,
    ApiServiceProxies.TimetableServiceProxy,
    ApiServiceProxies.UserServiceProxy
  ],
})
export class ServiceProxyModule {
}
