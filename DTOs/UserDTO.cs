using System.ComponentModel.DataAnnotations;

namespace BurgerMania.DTOs
{
    public class UserDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string phnNumber { get; set; }
        public string Password { get; set; }
    }
}
