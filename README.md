# eShopOnAbp

This project is a reference for who want to build microservice solutions with the ABP Framework.

> This project is in its infancy. By the time, we will write articles and documents to explain the goals and details of the project.

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

- Wait until all applications are up!

  - You can check running application from tye dashboard ([localhost:8000](http://127.0.0.1:8000/))

- After all your backend services are up, start the angular application:

  ```bash
  cd apps/angular
  yarn start
  ```

## Roadmap
### Development
- [x] New blank micro-service solution ✔️
- [x] Creating Deployment Scenarios ✔️
- [x] Creating empty business services ✔️

- [x] Implementing	 business logic into services ✔️
  - [x] Payment with PayPal ✔️
  - [x] Basket, Catalog, Order Service ✔️

- [ ] Switch ocelot to envoy in Public Web gateway
  - [ ] Use gRPC for catalog microservice

- [ ] Management Side of Services
- [ ] Administration Services & UI

### Deployment
- [x] Docker Image creation ✔️
- [ ] Docker compose for deployment
- [x] Helm deployment for local k8s cluster ✔️
- [ ] Deployment to azure k8s

### Documentation
- [ ] Create wiki
- [ ] Getting started
- [ ] Explore
- [ ] Deployment
---

Current milestone is Phase 1 structure is presented below:

![eSopOnAbp Phase 1](/docs/roadmap/Phase_1.png)

_See Phase 2 planning from [here](docs/roadmap/Phase_2.png)_

