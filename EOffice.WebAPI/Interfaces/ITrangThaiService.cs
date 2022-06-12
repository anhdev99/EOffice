using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces
{
    public interface ITrangThaiService
    {
        Task<TrangThai> Create(TrangThai model);
        Task<TrangThai> Update(TrangThai model);
        Task Delete(string id);
        Task<TrangThai> GetById(string id);
        Task<PagingModel<TrangThai>> GetPaging(TrangThaiParam param);
        Task<List<TrangThai>> GetAll();
        Task<List<TrangThaiTreeVM>> GetTree();
    }
}