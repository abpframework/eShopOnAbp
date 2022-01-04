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
    public Guid? PaymentRequestId { get; private set; }  
    public string PaymentStatus { get; private set; }  
    public Address Address { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public List<OrderItem> OrderItems { get; private set; }

    private Order()
    {
    }

    internal Order(Guid id, Address address, Guid? buyerId = null, Guid? paymentRequestId = null) : base(id)
    {
        _orderStatusId = OrderStatus.Placed.Id;
        OrderDate = DateTime.UtcNow;
        Address = address;
        BuyerId = buyerId;
        PaymentRequestId = paymentRequestId;
        PaymentStatus = "Waiting"; // TODO: magic string
        OrderItems = new List<OrderItem>();
    }

    internal Order SetOrderPaid()
    {
        OrderStatus = OrderStatus.Paid;

        return this;
    }

    public Order AddOrderItem(Guid id, Guid productId, string productName, string productCode, decimal unitPrice,
        decimal discount, string pictureUrl, int units = 1)
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
            var orderItem = new OrderItem(id, productId, productName, productCode, unitPrice, discount, pictureUrl, units);
            OrderItems.Add(orderItem);
        }

        return this;
    }

    public decimal GetTotal()
    {
        return OrderItems.Sum(o => o.Units * o.UnitPrice);
    }
}