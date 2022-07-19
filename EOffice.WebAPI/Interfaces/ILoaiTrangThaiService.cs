using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface ILoaiTrangThaiService
    {
        Task<LoaiTrangThai> Create(LoaiTrangThai model);
        Task<LoaiTrangThai> Update(LoaiTrangThai model);
        Task Delete(string id);
        Task<List<LoaiTrangThai>> Get();
        Task<LoaiTrangThai> GetById(string id);
        Task<PagingModel<LoaiTrangThai>> GetPaging(LinhVucParam param);
    }
}