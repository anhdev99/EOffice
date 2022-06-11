using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces.Identity
{
    public interface IModuleService
    {
        Task<Module> Create(Module model);
        Task<Module> Update(Module model);
        Task Delete(string id);
        Task<IEnumerable<Module>> Get();
        Task<Module> GetById(string id);
        Task<Module> AddPermissionToModule(Permission model);
        Task<Permission> GetPermissionById(Permission model);
        Task DeletePermission(Permission model);
        
        Task<PagingModel<Module>> GetPaging(LinhVucParam param);
        Task<List<ModuleTreeVM>> GetTreeModule();
    }
}