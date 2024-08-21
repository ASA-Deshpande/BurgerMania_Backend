using System.ComponentModel.DataAnnotations;

namespace BurgerMania.DTOs
{
    public class BurgerDTO
    {

        public Guid burgerId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
    }
}
