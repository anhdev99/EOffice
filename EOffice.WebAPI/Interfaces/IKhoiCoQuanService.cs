using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IKhoiCoQuanService
    {
        Task<KhoiCoQuan> Create(KhoiCoQuan model);
        Task<KhoiCoQuan> Update(KhoiCoQuan model);
        Task Delete(string id);
        Task<List<KhoiCoQuan>> Get();
        Task<KhoiCoQuan> GetById(string id);
        Task<PagingModel<KhoiCoQuan>> GetPaging(PagingParam param);
        Task ReadDataCoQuan(string filePath);
        Task ReadDataKhoiCoQuan(string filePath);
        Task ReadDataChucVu(string filePath);

    }
}