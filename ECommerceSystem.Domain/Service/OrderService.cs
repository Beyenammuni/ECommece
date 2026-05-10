using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.App.DTOs.ProductDtos.Response;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.IServices;


namespace ECommerceSystem.Domain.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unit;
        public async Task<Result<List<OrderDto>>> GetAllOrderAsync()
        {
            var orders = await _unit.Orders.GetAllAsync();

            var result = orders.Select(p => new OrderDto
            {
                CustomerId = p.CustomerId,
                OrderDate = p.OrderDate,
                TotalAmount = p.TotalAmount,
                Status = p.Status
            }).ToList();

            return Result<List<OrderDto>>.Success(result);
        }

        public async Task<Result<List<OrderModel>>> GetOrdersAsync(OrderFilterDto filter)
        {
            var orders = await _unit.Orders.GetAllAsync();

            if (filter.Status.HasValue)
                orders = orders.Where(o => o.Status == filter.Status.Value).ToList();
            if (filter.CustomerId.HasValue)
                orders = orders.Where(o => o.CustomerId == filter.CustomerId.Value).ToList();
            if (filter.FromDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= filter.FromDate.Value).ToList();
            if (filter.ToDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= filter.ToDate.Value).ToList();

            return Result<List<OrderModel>>
                .Success(orders);
        }

        public async Task<Result<bool>> Update(int id, UpdateOrder dto)
        {
            var order = await _unit.Orders.GetByIdAsync(id);
            if (order == null) 
                return Result<bool>.Failure("Order not found");
            if (order.Status == OrderStatus.Cancelled)
                return Result < bool>.Failure("You can not update a canceled order");
            if(order.Status == OrderStatus.Delivered)
                return Result<bool>.Failure("You can not update a delivered order");
            order.CustomerId = dto.CustomerId;
            order.OrderDate = dto.OrderDate;
            order.Status = dto.Status;
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto)
        {
            var order =await _unit.Orders.GetByIdAsync(orderId);

            if (order == null)
                return Result<bool>.Failure("Order not found");

            order.Status = dto.Status;

            await _unit.Orders.UpdateAsync(order);

            await _unit.Complete();

            return Result<bool>.Success(true);
        }
    }
}
