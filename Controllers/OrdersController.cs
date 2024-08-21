using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BurgerMania.Data;
using BurgerMania.Models;
using BurgerMania.DTOs;
using BurgerMania.Mappers;

namespace BurgerMania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly BurgerManiaContext _context;
        private readonly OrderMapper _orderMapper;

        public OrdersController(BurgerManiaContext context)
        {
            _context = context;
            _orderMapper = new OrderMapper();
        }

        //// GET: api/Orders
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
        //{
        //    return await _context.Orders.ToListAsync();
        //}

        //// GET: api/Orders/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Orders>> GetOrders(Guid id)
        //{
        //    var orders = await _context.Orders.FindAsync(id);

        //    if (orders == null)
        //    {
        //        return NotFound();
        //    }

        //    return orders;
        //}

        //// PUT: api/Orders/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrders(Guid id, Orders orders)
        //{
        //    if (id != orders.orderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(orders).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrdersExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Orders
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Orders>> PostOrders(Orders orders)
        //{
        //    orders.orderId = Guid.NewGuid();

        //    var user = _context.Users.Find(orders.UserID);

        //    if (user == null)
        //    {
        //        return NotFound($"User with userid: {orders.UserID} not Found!");
        //    }


        //    _context.Orders.Add(orders);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (OrdersExists(orders.orderId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetOrders", new { id = orders.orderId }, orders);
        //}

        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrders(Guid id)
        //{
        //    var orders = await _context.Orders.FindAsync(id);
        //    if (orders == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(orders);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrdersExists(Guid id)
        //{
        //    return _context.Orders.Any(e => e.orderId == id);
        //}


        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            var orderDTOs = orders.Select(order => _orderMapper.MapToDTO(order)).ToList();

            return Ok(orderDTOs);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrders(Guid id)
        {
            var orders = await _context.Orders.FindAsync(id);

            if (orders == null)
            {
                return NotFound();
            }

            var orderdto = _orderMapper.MapToDTO(orders);

            return Ok(orderdto);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(Guid id, OrderDTO orderdto)
        {
            if (id != orderdto.orderId)
            {
                return BadRequest();
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            if (orderdto.totAmount.HasValue)
            {
                order.totAmount = orderdto.totAmount.Value;
            }

            if (!string.IsNullOrEmpty(orderdto.status))
            {
                order.status = orderdto.status;
            }
            

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrders(OrderDTO orderdto)
        {
            orderdto.orderId = Guid.NewGuid();
            orderdto.status = "pending";
            orderdto.orderDate = DateTime.Now;

            var user = _context.Users.Find(orderdto.UserID);

            if (user == null)
            {
                return NotFound($"User with userid: {orderdto.UserID} not Found!");
            }

            var order = _orderMapper.MapToEntity(orderdto);

            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrdersExists(order.orderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrders", new { id = order.orderId }, orderdto);
        }



        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrders(Guid id)
        {
            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersExists(Guid id)
        {
            return _context.Orders.Any(e => e.orderId == id);
        }
    }
}
