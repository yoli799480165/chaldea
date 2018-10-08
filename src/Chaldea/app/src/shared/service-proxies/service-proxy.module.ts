import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';

@NgModule({
  providers: [
    ApiServiceProxies.GraphinstanceServiceProxy,
    ApiServiceProxies.GraphServiceProxy,
    ApiServiceProxies.ConsumerServiceProxy,
    ApiServiceProxies.SchemaServiceProxy,
    ApiServiceProxies.SourceServiceProxy,
  ],
})
export class ServiceProxyModule {
}
