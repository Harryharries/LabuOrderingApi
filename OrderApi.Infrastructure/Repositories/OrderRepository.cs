﻿using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entity;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    internal class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true,"Order created successfully") : new Response(false,"Failed to create order");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Failed to create order");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is null)
                {
                    return new Response(false,"Order not found");
                }
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true,"Order deleted successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Failed to delete order");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders.FindAsync(id);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Failed to find order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var order = await context.Orders.AsNoTracking().ToListAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Failed to get all orders");
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).ToListAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Failed to get all orders");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Failed to get order");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await context.Orders.FindAsync(entity.Id);
                if (order is null)
                {
                    return new Response(false,"Order not found");
                }
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true,"Order updated successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Failed to update order");
            }
        }
    }
}
