using EShopOnAbp.PaymentService.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public class PaymentRequestEfCoreRepository : EfCoreRepository<IPaymentServiceDbContext, PaymentRequest, Guid>, IPaymentRequestRepository
    {
        public PaymentRequestEfCoreRepository(IDbContextProvider<IPaymentServiceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
