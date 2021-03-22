using System.Threading.Tasks;
using FoodBook.Common.DTOs.UserDTOs;

namespace FoodBook.Business.Repositories.IRepositories
{
    public interface IUserRepo
    {
        #region UserMethods 
        Task<UserDTO> CreateUser(UserDTO userToAdd, string password);
        Task<UserDTO> GetUserById(string userId);
        Task<UserDTO> UpdateUser(UserDTO userToUpdate);
        Task<bool> DeleteUser(string userId);
        Task<LoginUserRespondDTO> LoginUser(string email, string password);

        #endregion
    }
}