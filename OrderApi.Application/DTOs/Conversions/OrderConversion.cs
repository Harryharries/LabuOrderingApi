using System;
using OrderApi.Domain.Entity;
namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new Order()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            PurchaseQuantity = order.PurchaseQuantity,
            OrderDate = order.OrderDate,
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order order, IEnumerable<Order>? orders)
        {
            if (order is not null || orders is null)
            {
                var singleOrder = new OrderDTO
                    (order!.Id,
                    order!.ClientId,
                    order!.ProductId,
                    order!.PurchaseQuantity,
                    order!.OrderDate
                    );
                return (singleOrder, null);            
            }
            if (orders is not null || order is null)
            {
                var _orders = orders!.Select(p => new OrderDTO(p.Id, p.ClientId, p.ProductId, p.PurchaseQuantity, p.OrderDate)).ToList();
                return (null, _orders);
            }

            return (null, null);
        }
    } 
}
