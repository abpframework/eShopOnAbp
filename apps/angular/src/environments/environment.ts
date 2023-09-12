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
    scope: 'offline_access openid profile email phone roles AdministrationService IdentityService BasketService CatalogService OrderingService PaymentService CmskitService', 
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
    }
  },
  mediaServerUrl:'https://localhost:44335'
} as MyEnvironment;


