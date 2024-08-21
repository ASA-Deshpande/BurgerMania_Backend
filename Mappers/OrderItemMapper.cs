using BurgerMania.DTOs;
using BurgerMania.Models;

namespace BurgerMania.Mappers
{
    public class OrderItemMapper
    {
        public OrderItemDTO MapToDTO(OrderItem orderitem)
        {
            if (orderitem == null)
                return null;

            var orderitemdto = new OrderItemDTO();
            orderitemdto.OrderID = orderitem.OrderID;
            orderitemdto.orderItemId = orderitem.orderItemId;
            orderitemdto.qty = orderitem.qty;
            orderitemdto.BurgerID = orderitem.BurgerID;
            //if (orderitem.Burger != null)
            //{
            //    orderitemdto.BurgerDTO = new BurgerDTO
            //    {
            //        burgerId = orderitem.BurgerID,
            //        Name = orderitem.Burger.Name,
            //        Category = orderitem.Burger.Category,
            //        Price = orderitem.Burger.Price,
            //    };
            //}


            return orderitemdto;
        }

        public OrderItem MapToEntity(OrderItemDTO orderitemdto)
        {
            if (orderitemdto == null)
                return null;

            var orderitem = new OrderItem();

            orderitem.orderItemId = orderitemdto.orderItemId;
            orderitem.OrderID = orderitemdto.OrderID;
            orderitem.BurgerID = orderitemdto.BurgerID;
            orderitem.qty = orderitemdto.qty;

            //    if(orderitemdto.BurgerDTO != null)
            //{
            //    orderitem.Burger = new Burger
            //    {
            //        burgerId = orderitemdto.BurgerID,
            //        Name = orderitemdto.BurgerDTO.Name,
            //        Category = orderitemdto.BurgerDTO.Category,
            //        Price = orderitemdto.BurgerDTO?.Price ?? (decimal?)null
            //    };

            //}

            return orderitem;

        }
    }
}
