using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface ICongVanService
    {
        Task<CongVan> Create(CongVan model);  
        Task<CongVan> Update(CongVan model);
        Task Delete(string id);
        Task<List<CongVan>> Get();
        Task<CongVan> GetById(string id);
        Task<PagingModel<CongVan>> GetPaging(CongVanParam param);
    }
}