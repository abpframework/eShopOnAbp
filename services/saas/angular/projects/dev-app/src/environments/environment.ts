import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'SaasService',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44348',
    redirectUri: baseUrl,
    clientId: 'SaasService_App',
    responseType: 'code',
    scope: 'offline_access SaasService role email openid profile',
  },
  apis: {
    default: {
      url: 'https://localhost:44348',
      rootNamespace: 'EShopOnAbp.SaasService',
    },
    SaasService: {
      url: 'https://localhost:44396',
      rootNamespace: 'EShopOnAbp.SaasService',
    },
  },
} as Environment;
