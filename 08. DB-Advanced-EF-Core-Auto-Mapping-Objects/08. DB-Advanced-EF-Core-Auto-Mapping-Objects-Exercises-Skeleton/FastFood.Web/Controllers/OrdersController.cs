﻿using AutoMapper.QueryableExtensions;
using FastFood.Models;
using FastFood.Models.Enums;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using Data;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items.Select(x => x.Name).ToList(),
                Employees = this.context.Employees.Select(x => x.Name).ToList(),
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var employee = this.context.Employees
                .FirstOrDefault(e => e.Name == model.EmployeeName);


            var item = this.context.Items
                .FirstOrDefault(i => i.Name == model.ItemName);

            var order = this.mapper.Map<Order>(model);
            order.DateTime = DateTime.Now;
            order.Type = Enum.Parse<OrderType>(model.OrderType);
            order.EmployeeId = employee.Id;
            order.OrderItems.Add(new OrderItem()
            {
                Order = order,
                ItemId = item.Id,
                Quantity = model.Quantity
            });

            this.context.Orders.Add(order);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = this.context.Orders
                .ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(orders);
        }
    }
}