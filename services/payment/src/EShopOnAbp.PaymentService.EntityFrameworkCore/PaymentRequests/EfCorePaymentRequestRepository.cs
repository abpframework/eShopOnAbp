using EShopOnAbp.PaymentService.EntityFrameworkCore;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public class EfCorePaymentRequestRepository :
        EfCoreRepository<
            IPaymentServiceDbContext,
            PaymentRequest,
            Guid>,
        IPaymentRequestRepository
    {
        public EfCorePaymentRequestRepository(IDbContextProvider<IPaymentServiceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
