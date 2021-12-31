using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace EShopOnAbp.OrderingService.Orders;

public class Order : AggregateRoot<Guid>
{
    private int _orderStatusId;
    public DateTime OrderDate { get; private set; }
    public Guid? BuyerId { get; private set; }
    public string PaymentMethodToken { get; private set; } // Payment token for validation 
    public Address Address { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public List<OrderItem> OrderItems { get; private set; }

    private Order()
    {
    }

    internal Order(Guid id, Address address, Guid? buyerId = null, string paymentMethodToken = null) : base()
    {
        _orderStatusId = OrderStatus.Placed.Id;
        OrderDate = DateTime.UtcNow;
        Address = address;
        BuyerId = buyerId;
        PaymentMethodToken = paymentMethodToken;
        OrderItems = new List<OrderItem>();
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl,
        int units = 1)
    {
        var existingOrderForProduct = OrderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.Discount)
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
            OrderItems.Add(orderItem);
        }
    }

    public decimal GetTotal()
    {
        return OrderItems.Sum(o => o.Units * o.UnitPrice);
    }
}