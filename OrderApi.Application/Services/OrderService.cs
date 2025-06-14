using eCommerce.SharedLibrary.Interface;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entity;
using Polly;
using Polly.Registry;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var getProductById = await httpClient.GetAsync($"/api/Product/{productId}");
            if (!getProductById.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getProductById.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }   

        public async Task<AppUserDTO> GetAppUserByIdAsync(int appUserId)
        {
            var getAppUserById = await httpClient.GetAsync($"/api/AppUser/{appUserId}");
            if (!getAppUserById.IsSuccessStatusCode)
            {
                return null!;
            }
            var appUser = await getAppUserById.Content.ReadFromJsonAsync<AppUserDTO>();
            return appUser!;
        }
        public async Task<IEnumerable<OrderDTO>> GetOrderByClientIdAsync(int clientId)
        {
            var orders = await orderInterface.GetAllOrdersAsync(p => p.ClientId == clientId);
            if (!orders.Any())
            {
                return null!;
            }

            var (_, _orders) = OrderConversion.FromEntity(null!, orders);
            return _orders!;
        }

        public async Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId)
        {
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null)
            {
                return null!;
            }
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProductByIdAsync(order.ProductId));

            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetAppUserByIdAsync(order.ClientId));
            return new OrderDetailsDTO(
                order.Id,
                productDTO!.Id,
                order.PurchaseQuantity,
                appUserDTO!.Id,
                appUserDTO!.PhoneNumber,
                appUserDTO!.Address,
                appUserDTO!.City,
                appUserDTO!.State,
                appUserDTO!.ZipCode,
                appUserDTO!.Country,
                appUserDTO!.Email,
                productDTO!.Price,
                order.PurchaseQuantity * productDTO!.Price,
                order.OrderDate,
                "",
                ""
            );
        }
    }
}
