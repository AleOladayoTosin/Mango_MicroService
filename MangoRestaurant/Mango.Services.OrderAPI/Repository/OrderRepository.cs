﻿using AutoMapper;
using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddOrder(OrderHeader order)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.orderHeaders.Add(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHearderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHearderFromDb = await _db.orderHeaders
                .FirstOrDefaultAsync(u => u.OrderHeaderId == orderHearderId);

            if(orderHearderFromDb != null)
            {
                orderHearderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }

        }
    }
}
