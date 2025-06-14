using eCommerce.SharedLibrary.Interface;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Entity;
using System.Linq.Expressions;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrderByClientIdAsync(int clientId);
        Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId);
    }
}
