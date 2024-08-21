using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BurgerMania.Data;
using BurgerMania.Models;
using Microsoft.EntityFrameworkCore;
using BurgerMania.Mappers;
using BurgerMania.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;

namespace BurgerMania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly BurgerManiaContext _context;
        private readonly OrderItemMapper _orderItemMapper;

        public OrderItemController(BurgerManiaContext context)
        {
            _context = context;
            _orderItemMapper = new OrderItemMapper();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderItem>>> GetAllItems()
        //{
        //    return await _context.Items.ToListAsync();
        //}

        //[HttpGet("/item/{itemid}")]
        //public async Task<ActionResult<OrderItem>> GetItemByID(Guid itemid)
        //{
        //    var item = await _context.Items.FindAsync(itemid);

        //    if(item == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(item);
        //    }
        //}

        //[HttpGet("/order/{orderid}")]
        //public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItemsByOrderID(Guid orderid)
        //{
        //    var orderItemsList = await _context.Items
        //        .Where(o => o.OrderID == orderid)
        //        .Include(b => b.Burger)
        //        .ToListAsync();

        //    if (orderItemsList == null || !orderItemsList.Any())
        //    {
        //        return NotFound($"No items found corresponding to order id: ${orderid}");
        //    }


        //    return Ok(orderItemsList);
        //}


        //[HttpPost]
        //public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem orderitem)
        //{

        //    orderitem.orderItemId = Guid.NewGuid();

        //    var existingItem = await GetItemByOrderIDAndBurgerID(orderitem.OrderID, orderitem.BurgerID);
        //    if (existingItem != null)
        //    {
        //        return Conflict("Trying to post duplicate order item");
        //    }

        //    _context.Items.Add(orderitem);
        //    await _context.SaveChangesAsync();

        //    //try
        //    //{
        //    //    await _context.SaveChangesAsync();
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    return StatusCode(500, "Error occured when saving burger to DB");
        //    //}

        //    return CreatedAtAction(nameof(GetItemByID), new  { itemID = orderitem.OrderID }, orderitem);
        //}

        //[HttpDelete]
        //public async Task<IActionResult> RemoveOrderItem(Guid orderid, Guid burgerid)
        //{
        //    var itemTBDeleted = await GetItemByOrderIDAndBurgerID(orderid, burgerid);

        //    if (itemTBDeleted == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Items.Remove(itemTBDeleted);

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return Conflict("Burger was modified by another user. Pls try again");

        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Something went wrong when deleting burger");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Unexpected Error occured");
        //    }

        //    return NoContent();

        //}

        //[HttpPut("{orderitemid}")]
        //public async Task<IActionResult> UpdateItem(Guid orderitemid, OrderItem item)
        //{
        //    if (orderitemid != item.orderItemId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(item).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderItemExists(orderitemid))
        //        {
        //            return NotFound($"OrderItem with id: {orderitemid} doesn't exist!");
        //        }
        //        else
        //        {
        //            return Conflict("Said orderitem has been modified by another user, please try again");
        //        }
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Error occured when saving the update to DB");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Something went wrong");
        //    }

        //    return NoContent();

        //}

        //private bool OrderItemExists(Guid orderitemid)
        //{
        //    return _context.Items.Any(i => i.orderItemId == orderitemid);
        //}

        //[NonAction]
        //public async Task<OrderItem> GetItemByOrderIDAndBurgerID(Guid orderid, Guid burgerid)
        //{
        //    var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.OrderID == orderid && i.BurgerID == burgerid);

        //    return existingItem;
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAllItems()
        {
            var items = await _context.Items.ToListAsync();

            var itemDTOs = items.Select(item => _orderItemMapper.MapToDTO(item)).ToList();

            return Ok(itemDTOs);
        }

        [HttpGet("/item/{itemid}")]
        public async Task<ActionResult<OrderItemDTO>> GetItemByID(Guid itemid)
        {
            var item = await _context.Items.FindAsync(itemid);

            var itemDTO = _orderItemMapper.MapToDTO(item);

            if (itemDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(itemDTO);
            }
        }

        [Authorize]
        [HttpGet("/order")]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrderItemsByOrderID()
        {

            // Prepare a list to hold all claims
            var claimsList = User.Claims.Select(claim => new
            {
                Type = claim.Type,
                Value = claim.Value
            }).ToList();


            // Log all claims to the console
            Console.WriteLine("All Claims:");
            foreach (var claim in claimsList)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }

            var phoneNumberClaim = User.FindFirst("sub");

            Console.WriteLine($"phoneNumberClaim: {phoneNumberClaim}");

            if (phoneNumberClaim == null)
            {
                return NotFound("Didn't find phn claim");
            }

            var phoneNumber = phoneNumberClaim.Value;


            var user = _context.Users.FirstOrDefault(u => u.phnNumber == phoneNumber);


            if (user == null)
            {
                return NotFound("User not found");
            }

            var userId = user.ID;

            var orderIdResult = await GetOrderIdCorrespondingtoUser(userId);
            var orderId = orderIdResult.Value;

            if (orderId == null)
            {
                return StatusCode(500, "Something went wrong");
            }

            var orderItemsList = await _context.Items
                .Where(o => o.OrderID == orderId)
                .Include(b => b.Burger)
                .ToListAsync();

            if (orderItemsList == null || !orderItemsList.Any())
            {
                return NotFound($"No items found corresponding to order id: ${orderId}");
            }

            var orderItemDTOList = orderItemsList.Select(orderitem => _orderItemMapper.MapToDTO(orderitem)).ToList();


            return Ok(orderItemDTOList);
        }

        [HttpGet("/orderidforuser/{userId}")]
        public async Task<ActionResult<Guid?>> GetOrderIdCorrespondingtoUser(Guid userId)
        {

            var user = _context.Users.Find(userId);


            if (user == null)
            {
                return NotFound("User not found");
            }

            var latestOrder = await _context.Orders
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.orderDate)
                .FirstOrDefaultAsync();

            if (latestOrder != null && latestOrder.status == "pending")
            {
                return latestOrder.orderId;
            }
            else
            {
                var newOrder = new Orders
                {
                    orderId = Guid.NewGuid(),
                    status = "pending",
                    orderDate = DateTime.Now,
                    UserID = userId
                };

                _context.Orders.Add(newOrder);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return (Guid?) null;
                }
                
                return newOrder.orderId;
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDTO>> CreateOrderItem(OrderItemDTO orderitemdto)
        {

            orderitemdto.orderItemId = Guid.NewGuid();

            var existingItem = await GetItemByOrderIDAndBurgerID
                (orderitemdto.OrderID, orderitemdto.BurgerID);

            if (existingItem != null)
            {
                return Conflict(new { message = "Order item already exists", orderItemId = existingItem.orderItemId, qty = existingItem.qty });
            }

            var order = _context.Orders.Find(orderitemdto.OrderID);

            if (order == null)
            {
                return NotFound($"Order with orderid: {orderitemdto.OrderID} not Found!");
            }

            var orderitem = _orderItemMapper.MapToEntity(orderitemdto);
            _context.Items.Add(orderitem);
            //await _context.SaveChangesAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occured when saving burger to DB {ex.Message}");
            }

            return CreatedAtAction(nameof(GetItemByID), new { itemID = orderitem.OrderID }, orderitemdto);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrderItem(Guid orderid, Guid burgerid)
        {
            var itemTBDeleted = await GetItemByOrderIDAndBurgerID(orderid, burgerid);

            if (itemTBDeleted == null)
            {
                return NotFound();
            }

            //var itemTBDeleted = _orderItemMapper.MapToEntity(itemTBDeletedDTO);

            _context.Items.Remove(itemTBDeleted);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Burger was modified by another user. Pls try again");

            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Something went wrong when deleting burger");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected Error occured");
            }

            return NoContent();

        }

        [HttpPut("{orderitemid}")]
        public async Task<IActionResult> UpdateItem(Guid orderitemid, OrderItemDTO orderitemdto)
        {
            if (orderitemid != orderitemdto.orderItemId)
            {
                return BadRequest();
            }

            var orderitem = _context.Items.Find(orderitemid);

            if (orderitem == null)
            {
                return NotFound();
            }

            orderitem.qty = orderitemdto.qty;

            _context.Entry(orderitem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(orderitemid))
                {
                    return NotFound($"OrderItem with id: {orderitemid} doesn't exist!");
                }
                else
                {
                    return Conflict("Said orderitem has been modified by another user, please try again");
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error occured when saving the update to DB");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }

            return NoContent();

        }

        private bool OrderItemExists(Guid orderitemid)
        {
            return _context.Items.Any(i => i.orderItemId == orderitemid);
        }

        [NonAction]
        public async Task<OrderItem> GetItemByOrderIDAndBurgerID(Guid orderid, Guid burgerid)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.OrderID == orderid && i.BurgerID == burgerid);

            //var existingItemDTO = _orderItemMapper.MapToDTO(existingItem);

            return existingItem;
        }
    }
}
