using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Text.Json.Serialization;

namespace BurgerMania.Models
{
    [Table("order", Schema = "BurgerShopSchemaFF")]
    public class Orders
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid orderId { get; set; }

        public string? status { get; set; }

        public DateTime? orderDate { get; set; }

        public decimal? totAmount { get; set; }

        public Guid UserID { get; set; }

        //[JsonIgnore]
        // every order corresponds to a User.
        public User? User { get; set; } = null!;

        //[JsonIgnore]

        // every order has many Order Items
        public ICollection<OrderItem>? OrderItems { get; set; } 
    }
}
