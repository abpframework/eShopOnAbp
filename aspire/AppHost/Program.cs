var builder = DistributedApplication.CreateBuilder(args);

var administrationService =
    builder.AddProject<Projects.EShopOnAbp_AdministrationService_HttpApi_Host>("administrationService");
var identityService = builder.AddProject<Projects.EShopOnAbp_IdentityService_HttpApi_Host>("identityService");
var basketService = builder.AddProject<Projects.EShopOnAbp_BasketService>("basketService");
var catalogService = builder.AddProject<Projects.EShopOnAbp_CatalogService_HttpApi_Host>("catalogService");
var cmsKitService = builder.AddProject<Projects.EShopOnAbp_CmskitService_HttpApi_Host>("cmsKitService");
var orderingService = builder.AddProject<Projects.EShopOnAbp_OrderingService_HttpApi_Host>("orderingService");
var paymentService = builder.AddProject<Projects.EShopOnAbp_PaymentService_HttpApi_Host>("paymentService");

// var publicWebApp = builder.AddProject<Projects.EShopOnAbp_PublicWeb>("public-web");

builder.Build().Run();
