using BurgerMania.DTOs;
using BurgerMania.Models;


namespace BurgerMania.Mappers
{
    public class LoginMapper
    {
        public LoginDTO MapToDTO(User user)
        {
            if (user == null)
                return null;

            return new LoginDTO
            {
                phnNumber = user.phnNumber,
                Password = user.PasswordHash,
            };
        }

        public User MapToEntity(LoginDTO logindto)
        {
            if (logindto == null)
                return null;

            return new User
            {
                phnNumber = logindto.phnNumber,
                PasswordHash = logindto.Password,
            };

        }
    }
}
