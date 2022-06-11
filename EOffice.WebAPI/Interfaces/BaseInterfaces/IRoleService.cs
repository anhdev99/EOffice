using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<Role> Create(Role model);
        Task<Role> Update(Role model);
        Task Delete(string id);
        Task<IEnumerable<Role>> Get();
        Task<Role> GetById(string id);
        Task<PagingModel<Role>> GetPaging(PagingParam param);
        Task<List<NavMenuVM>> GetMenuForUser(string userName);
        Task<List<string>> GetPermissionForCurrentUer(string userName);
    }
}