param ($version='latest')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../../"
$webAppFolder = Join-Path $slnFolder "apps/angular"
$authserverFolder = Join-Path $slnFolder "apps/auth-server/src/EShopOnAbp.AuthServer"
$publicWebFolder = Join-Path $slnFolder "apps/public-web/src/EShopOnAbp.PublicWeb"

$webGatewayFolder = Join-Path $slnFolder "gateways/web/src/EShopOnAbp.WebGateway"
$publicWebGatewayFolder = Join-Path $slnFolder "gateways/web-public/src/EShopOnAbp.WebPublicGateway"
$internalGatewayFolder = Join-Path $slnFolder "gateways/internal/src/EShopOnAbp.InternalGateway"

$identityServiceFolder = Join-Path $slnFolder "services/identity/src/EShopOnAbp.IdentityService.HttpApi.Host"
$administrationServiceFolder = Join-Path $slnFolder "services/administration/src/EShopOnAbp.AdministrationService.HttpApi.Host"
$saasServiceFolder = Join-Path $slnFolder "services/saas/src/EShopOnAbp.SaasService.HttpApi.Host"

### Angular WEB App(WWW)
Write-Host "*** BUILDING WEB (WWW) ****************" -ForegroundColor Green
Set-Location $webAppFolder
yarn
# ng build --prod
npm run build --configuration=prod
docker build -t eshoponabp/app-web:$version .

### AUTH-SERVER
Write-Host "*** BUILDING AUTH-SERVER ****************" -ForegroundColor Green
Set-Location $authserverFolder
dotnet publish -c Release
docker build -t eshoponabp/app-authserver:$version .

### PUBLIC-WEB
Write-Host "*** BUILDING PUBLIC-WEB ****************" -ForegroundColor Green
Set-Location $publicWebFolder
dotnet publish -c Release
docker build -t eshoponabp/app-publicweb:$version .

### WEB-GATEWAY
Write-Host "*** BUILDING WEB-GATEWAY ****************" -ForegroundColor Green
Set-Location $webGatewayFolder
dotnet publish -c Release
docker build -t eshoponabp/gateway-web:$version .

### PUBLICWEB-GATEWAY
Write-Host "*** BUILDING PUBLICWEB-GATEWAY ****************" -ForegroundColor Green
Set-Location $publicWebGatewayFolder
dotnet publish -c Release
docker build -t eshoponabp/gateway-publicweb:$version .

### INTERNAL-GATEWAY
Write-Host "*** BUILDING INTERNAL-GATEWAY ****************" -ForegroundColor Green
Set-Location $internalGatewayFolder
dotnet publish -c Release
docker build -t eshoponabp/gateway-internal:$version .

### IDENTITY-SERVICE
Write-Host "*** BUILDING IDENTITY-SERVICE ****************" -ForegroundColor Green
Set-Location $identityServiceFolder
dotnet publish -c Release
docker build -t eshoponabp/service-identity:$version .

### ADMINISTRATION-SERVICE
Write-Host "*** BUILDING ADMINISTRATION-SERVICE ****************" -ForegroundColor Green
Set-Location $administrationServiceFolder
dotnet publish -c Release
docker build -t eshoponabp/service-administration:$version .

### SAAS-SERVICE
Write-Host "*** BUILDING SAAS-SERVICE ****************" -ForegroundColor Green
Set-Location $saasServiceFolder
dotnet publish -c Release
docker build -t eshoponabp/service-saas:$version .

### ALL COMPLETED
Write-Host "ALL COMPLETED" -ForegroundColor Green
Set-Location $currentFolder