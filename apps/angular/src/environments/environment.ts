import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'EShopOnAbp',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44330',
    redirectUri: baseUrl,
    clientId: 'EShopOnAbp_Angular',
    responseType: 'code',
    scope:
      'offline_access openid profile email phone IdentityService AdministrationService SaasService',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44372',
      rootNamespace: 'EShopOnAbp',
    }
  },
} as Environment;
