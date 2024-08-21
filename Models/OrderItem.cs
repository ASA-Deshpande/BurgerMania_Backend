using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace BurgerMania.Models
{
    [Table("orderItem", Schema = "BurgerShopSchemaFF")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid orderItemId { get; set; }
        public int qty { get; set; }

        // every orderItem has an order
        public Guid OrderID { get; set; }
        public Orders? Orders { get; set; }

        // every orderitem has just one burger
        public Guid BurgerID { get; set; }
        public Burger? Burger { get; set; } = null!;
    }
}
