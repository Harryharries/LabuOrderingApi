using eCommerce.SharedLibrary.Interface;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Entity;
using System.Linq.Expressions;

namespace OrderApi.Application.Interfaces
{
    public interface IOrder : IGernericInterface<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
