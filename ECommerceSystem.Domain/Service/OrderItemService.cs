using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.DTOs.OrderItemDtos.Request;
using ECommerceSystem.Domain.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.Service
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unit;

        public OrderItemService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<OrderItemModel>> CreateOrderForCustomerAsync(int CustomerId, CreateOrderItemSto dto)
        {
           var user = await _unit.OrderItems.GetByIdAsync(CustomerId);
            if (user == null)
            {
                return Result<OrderItemModel>.NotFound($"Customer with ID {CustomerId} not found.");
            }
            var orderItem = new OrderItemModel
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };
            await _unit.OrderItems.AddAsync(orderItem);
            await _unit.Complete();
            var orderItemDto = new OrderItemModel
            {
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
            return Result<OrderItemModel>.Success(orderItemDto, "Order item created successfully.");
        }

        public async Task<Result<bool>> DeleteOrderItemAsync(int id)
        {
            var orderItem = await _unit.OrderItems.GetByIdAsync(id);
            if (orderItem == null)
            {
                return Result<bool>.NotFound($"Order item with ID {id} not found.");
            }
            _unit.OrderItems.Delete(orderItem);
            await _unit.Complete();
            return Result<bool>.Success(true, "Order item deleted successfully.");
        }

        public async Task<Result<List<OrderItemDto>>> GetAllOrderItemsAsync()
        {
            var orderItems = await _unit.OrderItems.GetAllAsync();
            var result = orderItems.Select(oi => new OrderItemDto
            {
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList();
            return Result<List<OrderItemDto>>.Success(result);
        }

        public async Task<Result<OrderItemDto>> GetOrderItemByIdAsync(int id)
        {
            var orderItem = _unit.OrderItems.GetByIdAsync(id);
            if (orderItem == null)
            {
                return Result<OrderItemDto>.NotFound($"Order item with ID {id} not found.");
            }
            var orderItemDto = new OrderItemDto
            {
                OrderId = orderItem.Result.OrderId,
                ProductId = orderItem.Result.ProductId,
                Quantity = orderItem.Result.Quantity,
                UnitPrice = orderItem.Result.UnitPrice
            };
            return Result<OrderItemDto>.Success(orderItemDto);
        }

        public async Task<Result<OrderItemModel>> UpdateOrderItemAsync(int id, UpdateOrderItem dto)
        {
            var orderItem = await _unit.OrderItems.GetByIdAsync(id);
            if (orderItem == null)
            {
                return Result<OrderItemModel>.NotFound($"Order item with ID {id} not found.");
            }
            if(orderItem.OrderId != dto.OrderId || orderItem.ProductId != dto.ProductId)
            {
                return Result<OrderItemModel>.BadRequest("Order ID and Product ID cannot be changed.");
            }
            if(dto.Quantity <= 0)
            {
                return Result<OrderItemModel>.BadRequest("Quantity must be greater than zero.");
            }
            orderItem.Quantity = dto.Quantity;
            orderItem.UnitPrice = dto.UnitPrice;
            _unit.OrderItems.Update(orderItem);
            await _unit.Complete();
            var orderItemDto = new OrderItemModel
            {
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
            return Result<OrderItemModel>.Success(orderItemDto, "Order item updated successfully.");

        }


    }
}
