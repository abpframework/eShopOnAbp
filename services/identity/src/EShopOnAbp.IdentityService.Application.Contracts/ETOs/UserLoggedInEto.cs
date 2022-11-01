using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace EShopOnAbp.IdentityService.ETOs
{
    [EventName("EShopOnAbp.Identity.UserLoggedIn")]
    [Serializable]
    public class UserLoggedInEto : EtoBase
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}