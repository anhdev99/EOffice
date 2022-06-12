using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(User model);
        Task<User> Update(User model);
        Task Delete(string id);
        Task<IEnumerable<User>> Get();
        Task<User> GetById(string id);
        Task<PagingModel<User>> GetPaging(PagingParam param);
        Task<User> GetByUserName(string userName);  
        Task<User> ChangePassword(UserVM model);
        Task<User> ChangeProfile(User model);
        Task<User> FindUserWithUserNameOrPhoneNumber(string input);
        Task<List<UserTreeVM>> GetTree();

    }
}