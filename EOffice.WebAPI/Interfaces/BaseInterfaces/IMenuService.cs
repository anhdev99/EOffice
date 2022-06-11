using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces.Identity
{
    public interface IMenuService
    {
        Task<Menu> Create(Menu model);
        Task<Menu> Update(Menu model);
        Task Delete(string id);
        Task<IEnumerable<Menu>> Get();
        Task<Menu> GetById(string id);
        Task<PagingModel<Menu>> GetPaging(PagingParam param);
        Task<List<MenuTreeVM>> GetTree();
        Task<List<MenuTreeVM>> GetTreeList();
        Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu);
    }
}