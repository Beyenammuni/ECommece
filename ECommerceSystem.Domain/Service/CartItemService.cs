using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.CartItemDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.IServices;


namespace ECommerceSystem.Domain.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unit;

        public CartItemService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<CartItemModel>> CreateCartItemAsync(int CustomerId, CreateCartItem dto)
        {
            var cartItem = new CartItemModel
            {
                CustomerId = CustomerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            if (cartItem.Quantity <= 0)
                return Result<CartItemModel>.Failure("Quantity must be greater than zero.");
            if(cartItem.Quantity < dto.Quantity)
                return Result<CartItemModel>.Failure("Requested quantity exceeds available stock.");
            if (cartItem.CustomerId <= 0)
                return Result<CartItemModel>.Failure("Invalid Customer ID.");
            if (cartItem.ProductId <= 0)      
                return Result<CartItemModel>.Failure("Invalid Product ID.");


            await _unit.CartItems.AddAsync(cartItem);
            await _unit.Complete();
            return Result<CartItemModel>.Success(cartItem, "Cart item created successfully.");
        }

        public async Task<Result<bool>> DeleteCartItemAsync(int id)
        {
            var cartItem = await _unit.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                return Result<bool>.Failure("Cart item not found.");

             _unit.CartItems.Delete(cartItem);
            await _unit.Complete();

            return Result<bool>.Success(true, "Cart item deleted successfully.");
        }

        public async Task<Result<List<CartItemDto>>> GetCstomerCartItemAsync(int CustomerId)
        {
            var cartItems = await _unit.CartItems.GetCustomerCartAsync(CustomerId);

            var result = cartItems.Select(c => new CartItemDto
            {               
                CustomerId = c.CustomerId,
                ProductId = c.ProductId,
                Quantity = c.Quantity
            }).ToList();
            return Result<List<CartItemDto>>.Success(result);
        }

        public async Task<Result<bool>> UpdateCartItemAsync(int id, UpdateCartItem dto)
        {
            var cartItem = await _unit.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                return Result<bool>.Failure("Cart item not found.");
            cartItem.CustomerId = dto.CustomerId;
            cartItem.ProductId = dto.ProductId;
            cartItem.Quantity = dto.Quantity;

             _unit.CartItems.Update(cartItem);
             await _unit.Complete();

            return Result<bool>.Success(true, "Cart item updated successfully.");
        }

    }
}
