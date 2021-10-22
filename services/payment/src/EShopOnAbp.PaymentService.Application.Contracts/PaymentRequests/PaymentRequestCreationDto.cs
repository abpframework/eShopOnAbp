using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    [Serializable]
    public class PaymentRequestCreationDto
    {
        [Required]
        [MaxLength(PaymentRequestConsts.MaxCurrencyLength)]
        public string Currency { get; set; }

        public string BuyerId { get; set; }

        public List<PaymentRequestProductCreationDto> Products { get; set; }
    }
}