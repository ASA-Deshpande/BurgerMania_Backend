using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace BurgerMania.Models
{
    [Table("user", Schema = "BurgerShopSchemaFF")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string phnNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // every user has Orders
        public ICollection<Orders>? Orders { get; } = new List<Orders>();


    }
}
