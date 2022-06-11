using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IChucVuService
    {
        Task<ChucVu> Create(ChucVu model);
        Task<ChucVu> Update(ChucVu model);
        Task Delete(string id);
        Task<List<ChucVu>> Get();
        Task<ChucVu> GetById(string id);
        Task<PagingModel<ChucVu>> GetPaging(LinhVucParam param);
    }
}