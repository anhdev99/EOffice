using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface ILichCongTacService
    {
        Task<LichCongTac> Create(LichCongTac model);
        Task<LichCongTac> Update(LichCongTac model);
        Task Delete(string id);
        Task<List<LichCongTac>> GetByDateNow();
        Task<List<LichCongTac>> Get();
        Task<LichCongTac> GetById(string id);
        Task<PagingModel<LichCongTac>> GetPaging(PagingParam param);
        Task<List<LichCongTac>> GetByDate(PagingParamDate param);
    }
}