using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace BurgerMania.Models
{
    [Table("burger", Schema = "BurgerShopSchemaFF")]
    public class Burger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid burgerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Category { get; set; }

        public decimal? Price { get; set; }

        // every burger can be there for multiple order items, or not :)
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

    }
}
