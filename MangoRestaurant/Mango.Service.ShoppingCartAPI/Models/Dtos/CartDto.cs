﻿namespace Mango.Service.ShoppingCartAPI.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> cartDetails { get; set; }
    }
}
