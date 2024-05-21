var builder = DistributedApplication.CreateBuilder(args);

var launchProfile = "HTTP";
// Microservices
var administrationService =
    builder.AddProject<Projects.EShopOnAbp_AdministrationService_HttpApi_Host>("administrationService",launchProfile);
var identityService = builder.AddProject<Projects.EShopOnAbp_IdentityService_HttpApi_Host>("identityService");
var catalogService = builder.AddProject<Projects.EShopOnAbp_CatalogService_HttpApi_Host>("catalogService")
    .AsHttp2Service();
var basketService = builder.AddProject<Projects.EShopOnAbp_BasketService>("basketService")
    .WithReference(catalogService); //?
var cmsKitService = builder.AddProject<Projects.EShopOnAbp_CmskitService_HttpApi_Host>("cmsKitService");
var orderingService = builder.AddProject<Projects.EShopOnAbp_OrderingService_HttpApi_Host>("orderingService");
var paymentService = builder.AddProject<Projects.EShopOnAbp_PaymentService_HttpApi_Host>("paymentService");

// Gateways
var webGateway = builder.AddProject<Projects.EShopOnAbp_WebGateway>("webGateway");
var webPublicGateway = builder.AddProject<Projects.EShopOnAbp_WebPublicGateway>("webPublicGateway")
    .WithReference(administrationService)
    .WithReference(identityService)
    .WithReference(catalogService)
    .WithReference(basketService)
    .WithReference(cmsKitService)
    .WithReference(orderingService)
    .WithReference(paymentService);

// Apps
var publicWebApp = builder.AddProject<Projects.EShopOnAbp_PublicWeb>("public-web")
    .WithExternalHttpEndpoints();

builder.Build().Run();
