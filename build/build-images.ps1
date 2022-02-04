param ($version='1.0')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../"
$webAppFolder = Join-Path $slnFolder "apps/angular"
$authserverFolder = Join-Path $slnFolder "apps/auth-server/src/EShopOnAbp.AuthServer"
$publicWebFolder = Join-Path $slnFolder "apps/public-web/src/EShopOnAbp.PublicWeb"

$webGatewayFolder = Join-Path $slnFolder "gateways/web/src/EShopOnAbp.WebGateway"
$webPublicGatewayFolder = Join-Path $slnFolder "gateways/web-public/src/EShopOnAbp.WebPublicGateway"

$identityServiceFolder = Join-Path $slnFolder "services/identity/src/EShopOnAbp.IdentityService.HttpApi.Host"
$administrationServiceFolder = Join-Path $slnFolder "services/administration/src/EShopOnAbp.AdministrationService.HttpApi.Host"
$catalogServiceFolder = Join-Path $slnFolder "services/catalog/src/EShopOnAbp.CatalogService.HttpApi.Host"
$paymentServiceFolder = Join-Path $slnFolder "services/payment/src/EShopOnAbp.PaymentService.HttpApi.Host"
$orderingServiceFolder = Join-Path $slnFolder "services/ordering/src/EShopOnAbp.OrderingService.HttpApi.Host"

Write-Host "===== BUILDING APPLICATIONS =====" -ForegroundColor Yellow

### Angular WEB App
Write-Host "*** BUILDING ANGULAR WEB APPLICATION 1/10 ***" -ForegroundColor Green
Set-Location $webAppFolder
docker build -f "$webAppFolder/Dockerfile" -t eshoponabp/app-web:$version .

### AUTH-SERVER
Write-Host "**************** BUILDING AUTH-SERVER 2/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$authserverFolder/Dockerfile" -t eshoponabp/app-authserver:$version .

### PUBLIC-WEB
Write-Host "**************** BUILDING WEB-PUBLIC 3/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$publicWebFolder/Dockerfile" -t eshoponabp/app-publicweb:$version .

Write-Host "===== BUILDING GATEWAYS =====" -ForegroundColor Yellow 

### WEB-GATEWAY
Write-Host "**************** BUILDING WEB-GATEWAY 4/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$webGatewayFolder/Dockerfile" -t eshoponabp/gateway-web:$version .

### PUBLICWEB-GATEWAY
Write-Host "**************** BUILDING WEB-PUBLIC-GATEWAY 5/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$webPublicGatewayFolder/Dockerfile" -t eshoponabp/gateway-web-public:$version .

Write-Host "===== BUILDING MICROSERVICES =====" -ForegroundColor Yellow

### IDENTITY-SERVICE
Write-Host "**************** BUILDING IDENTITY-SERVICE 6/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$identityServiceFolder/Dockerfile" -t eshoponabp/service-identity:$version .

### ADMINISTRATION-SERVICE
Write-Host "**************** BUILDING ADMINISTRATION-SERVICE 7/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$administrationServiceFolder/Dockerfile" -t eshoponabp/service-administration:$version .

### CATALOG-SERVICE
Write-Host "**************** BUILDING CATALOG-SERVICE 8/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$catalogServiceFolder/Dockerfile" -t eshoponabp/service-catalog:$version .

### PAYMENT-SERVICE
Write-Host "**************** BUILDING PAYMENT-SERVICE 9/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$paymentServiceFolder/Dockerfile" -t eshoponabp/service-payment:$version .

### ORDERING-SERVICE
Write-Host "**************** BUILDING ORDERING-SERVICE 10/10 ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$orderingServiceFolder/Dockerfile" -t eshoponabp/service-ordering:$version .

### ALL COMPLETED
Write-Host "ALL COMPLETED" -ForegroundColor Green
Set-Location $currentFolder