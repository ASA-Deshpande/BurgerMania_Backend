using BurgerMania.DTOs;
using BurgerMania.Models;

namespace BurgerMania.Mappers
{
    public class BurgerMapper
    {
        public BurgerDTO MapToDTO(Burger burger)
        {
            if(burger == null)
            {
                return null;
            }

            var burgerDTO = new BurgerDTO();

            burgerDTO.Name = burger.Name;
            burgerDTO.Category = burger.Category;
            burgerDTO.Price = burger.Price;
            burgerDTO.burgerId = burger.burgerId;

            return burgerDTO;

        }

        public Burger MapToEntity(BurgerDTO burgerdto)
        {
            if(burgerdto == null)
            {
                return null;
            }

            return new Burger
            {
                Name = burgerdto.Name,
                Category = burgerdto.Category,
                Price = burgerdto.Price ?? (decimal?) null,
                burgerId = burgerdto.burgerId
            };
        }
    }
}
