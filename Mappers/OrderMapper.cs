using BurgerMania.DTOs;
using BurgerMania.Models;

namespace BurgerMania.Mappers
{
    public class OrderMapper
    {
        public OrderDTO MapToDTO(Orders order)
        {
            if (order == null)
                return null;

            return new OrderDTO
            {
                orderId = order.orderId,
                UserID = order.UserID,
                totAmount = order.totAmount,
                orderDate = order.orderDate,
                status = order.status,
            };
        }

        public Orders MapToEntity(OrderDTO orderdto)
        {
            if (orderdto == null) return null;

            Orders order = new Orders();

            order.orderId = orderdto.orderId;
            order.UserID = orderdto.UserID;
            order.totAmount = orderdto.totAmount?? (decimal?)null;
            order.status = orderdto.status?? null;
            order.orderDate = orderdto.orderDate?? (DateTime?)null;

            return order;
        }

    }
}
