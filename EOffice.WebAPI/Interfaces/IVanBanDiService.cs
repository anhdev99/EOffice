using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

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

        Task<VanBanDi> AssignSign(PhanCongKySo model);
        Task<List<PhanCongKySo>> GetPhanCongKySoByVanBanId(string vanBanId);
        Task<VanBanDi> RemoveAssignSign(PhanCongKySo model);
        Task<VanBanDi> AssignOrReject(PhanCongKySo model, string path);
        Task<PagingModel<VanBanDi>> GetPagingXuLy(VanBanDiParam param);
        Task<VanBanDi> CapSoVanBan();
        Task<PagingModel<VanBanDi>> GetPagingCapSo(VanBanDiParam param);
        Task<VanBanDi> CapSoVanBanDi(VanBanDi model);
        Task XacThuc(XacMinhVM model);
        Task ChuyenTrangThaiVanBan(TrangThaiParam model);
        Task ThietLapKySoPhapLy(SignDigitalVM model);
        Task KySoPhapLy(SignDigitalVM model);
    }
}