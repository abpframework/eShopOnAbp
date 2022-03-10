# eShopOnAbp

This project is a reference for who want to build microservice solutions with the ABP Framework.

See [the announcement](https://blog.abp.io/abp/Introducing-the-eShopOnAbp-Project) post for more info.

## Issues

Please open issues on the main GitHub repository: https://github.com/abpframework/abp/issues

## How to Run?

You can either run in Visual Studio, or using [Microsoft Tye](https://github.com/dotnet/tye). Tye is a developer tool that makes developing, testing, and deploying micro-services and distributed applications easier.

 ### Requirements

- .NET 6.0+
- Docker
- Yarn

### Instructions

- Clone the repository ( [eShopOnAbp](https://github.com/abpframework/eShopOnAbp) )

- Install Tye (*follow [these steps](https://github.com/dotnet/tye/blob/main/docs/getting_started.md#installing-tye)*)

- Rename `.env.example` file to `.env` file and provide PayPal ClientID and Secret.

- Execute `run-tye.ps1`
- **Note**: If you meet with some shell issues regarding authorization. You may check with the following codes your access and then set your machine configuration. [for more detail](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.security/get-executionpolicy?view=powershell-7.2)
```bash
Get-ExecutionPolicy list
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine
 ```
- Wait until all applications are up!

  - You can check running application from tye dashboard ([localhost:8000](http://127.0.0.1:8000/))

- After all your backend services are up, start the angular application:

  ```bash
  cd apps/angular
  yarn start
  ```

## Roadmap

- [x] New blank micro-service solution ✔️
- [x] Creating Deployment Scenarios ✔️
- [x] Creating empty business services ✔️
- [x] Implementing	 business logic into services ✔️
  - [x] Payment with PayPal ✔️
  - [x] Basket, Catalog, Order Service ✔️
- [x] Docker Image creation ✔️
- [x] Helm deployment for local k8s cluster ✔️
- [ ] Switch Ocelot to Envoy in Public Web gateway
- [x] Use gRPC for catalog microservice
- [ ] Management Side of Services
- [ ] Administration application (to manage products and orders, with a dashboard)
- [ ] Product-detail page on the shopping application (with CMS-kit integration for comments and rating components)
- [ ] Deployment to azure k8s

### Documentation

- [ ] We will create an e-book to explain the solution

## Current Architecture

![eSopOnAbp Phase 1](/docs/roadmap/Phase_1.png)

## ABP Community Talks

We've organized a meetup related to this solution. You can watch it here: https://community.abp.io/posts/abp-community-talks-2022.1-microservice-development-a98jnsa0
