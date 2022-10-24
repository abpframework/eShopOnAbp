import { MyEnvironment } from './my-environment';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'EShopOnAbp',
  },
  oAuthConfig: {
    issuer: 'http://localhost:8080/realms/master',
    redirectUri: baseUrl,
    clientId: 'Web',
    responseType: 'code',
    scope: 'offline_access openid profile email phone',
    // scope: 'offline_access openid profile email phone AccountService IdentityService AdministrationService CatalogService OrderingService', //TODO: Update when https://github.com/AnderssonPeter/Keycloak.Net/pull/5 is merged
    //requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44372',
      rootNamespace: 'EShopOnAbp',
    },
    Catalog: {
      url: 'https://localhost:44354',
      rootNamespace: 'EShopOnAbp.CatalogService',
    },
    Ordering: {
      url: "https://localhost:44356",
      rootNamespace: 'EShopOnAbp.OrderingService',
    },
    Cmskit: {
        url: "https://localhost:44358",
        rootNamespace: 'EShopOnAbp.CmskitService',
    }
  },
  mediaServerUrl:'https://localhost:44335'
} as MyEnvironment;


