using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces
{
    public interface IVanBanDenService 
    {
        Task<VanBanDen> Create(VanBanDen model);  
        Task<VanBanDen> Update(VanBanDen model);
        Task Delete(string id);
        Task<List<VanBanDen>> Get();
        Task<VanBanDen> GetById(string id);
        Task<PagingModel<VanBanDen>> GetPaging(VanBanDenParam param);
        Task<PagingModel<VanBanDenVM>> GetPagingVM(VanBanDenParam param);
    }
}