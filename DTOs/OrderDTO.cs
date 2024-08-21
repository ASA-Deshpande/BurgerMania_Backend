namespace BurgerMania.DTOs
{
    public class OrderDTO
    {
        public Guid orderId { get; set; }
        public string? status { get; set; }
        public DateTime? orderDate { get; set; }
        public decimal? totAmount { get; set; }
        public Guid UserID { get; set; }

    }
}
