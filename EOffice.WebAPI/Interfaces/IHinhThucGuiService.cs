using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IHinhThucGuiService
    {
        Task<HinhThucGui> Create(HinhThucGui model);
        Task<HinhThucGui> Update(HinhThucGui model);
        Task Delete(string id);
        Task<List<HinhThucGui>> Get();
        Task<HinhThucGui> GetById(string id);
        Task<PagingModel<HinhThucGui>> GetPaging(PagingParam param);
    }
}