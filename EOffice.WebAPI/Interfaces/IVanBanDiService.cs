using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IVanBanDiService
    {
        Task<VanBanDi> Create(VanBanDi model);  
        Task<VanBanDi> Update(VanBanDi model);
        Task Delete(string id);
        Task<List<VanBanDi>> Get();
        Task<VanBanDi> GetById(string id);
        Task<PagingModel<VanBanDi>> GetPaging(VanBanDiParam param);
        Task<PagingModel<VanBanDi>> GetPagingUser(VanBanDiParam param);

        Task<VanBanDi> PhanCong(VanBanDi model);
        // Task ChangeStatusQuestion(StatusVanBan model);
        //
        // Task<PagingModel<VanBanDi>>   GetPagingReceive (VanBanDiParam param);
        // Task<PagingModel<VanBanDi>>   GetPagingHandle (VanBanDiParam param);
        //
        // Task<VanBanDi> QuestionNavigation(VanBanDi model);    
        // Task<VanBanDi> NotApprove(VanBanDi model);   
    }
}