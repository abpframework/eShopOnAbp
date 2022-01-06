using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Orders;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace EShopOnAbp.OrderingService
{
    public class OrderingServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly OrderManager _orderManager;
        private readonly TestData _testData;
        private readonly TestProducts _testProducts;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public OrderingServiceDataSeedContributor(
            OrderManager orderManager,
            TestData testData,
            TestProducts testProducts, UnitOfWorkManager unitOfWorkManager)
        {
            _orderManager = orderManager;
            _testData = testData;
            _testProducts = testProducts;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedTestOrdersAsync();
        }

        private async Task SeedTestOrdersAsync()
        {
            var randomOrderItems = _testProducts.GetRandomProducts(5);

            using (_unitOfWorkManager.Begin())
            {
                var order1 = await _orderManager.CreateOrderAsync(
                    1,
                    _testData.CurrentUserId,
                    _testData.CurrentUserName,
                    _testData.CurrentUserEmail,
                    randomOrderItems,
                    _testData.Address.Street,
                    _testData.Address.City,
                    _testData.Address.Country,
                    _testData.Address.ZipCode
                );
            }

            var order2 = await _orderManager.CreateOrderAsync(
                1, _testData.CurrentUserId, _testData.CurrentUserName, _testData.CurrentUserEmail,
                _testProducts.GetRandomProducts(10),
                _testData.Address.Street,
                _testData.Address.City,
                _testData.Address.Country,
                _testData.Address.ZipCode
            );
        }
    }
}