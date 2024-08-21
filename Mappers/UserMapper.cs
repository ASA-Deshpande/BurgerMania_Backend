using BurgerMania.DTOs;
using BurgerMania.Models;

namespace BurgerMania.Mappers
{
    public class UserMapper
    {

        public UserDTO MapToDTO(User user)
        {
            if (user == null)
                return null;

            return new UserDTO
            {
                Name = user.Name,
                phnNumber = user.phnNumber,
                ID = user.ID,
                Password = user.PasswordHash,
            };
        }

        public User MapToEntity(UserDTO userdto)
        {
            if (userdto == null)
                return null;

            return new User 
            { 
              ID = userdto.ID, 
              Name = userdto.Name,
              phnNumber = userdto.phnNumber,
              PasswordHash=userdto.Password,
            };   

        }
    }
}
