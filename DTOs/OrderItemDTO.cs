using BurgerMania.Models;

namespace BurgerMania.DTOs
{
    public class OrderItemDTO
    {
        public Guid orderItemId { get; set; }
        public int qty { get; set; }
        public Guid OrderID { get; set; }
        public Guid BurgerID { get; set; }
        //public BurgerDTO? BurgerDTO { get; set; } = null!;
    }
}
