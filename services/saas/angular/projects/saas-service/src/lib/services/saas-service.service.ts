import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class SaasServiceService {
  apiName = 'SaasService';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/SaasService/sample' },
      { apiName: this.apiName }
    );
  }
}
