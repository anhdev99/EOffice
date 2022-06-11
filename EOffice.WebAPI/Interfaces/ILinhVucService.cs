using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface ILinhVucService
    {
        Task<LinhVuc> Create(LinhVuc model);
        Task<LinhVuc> Update(LinhVuc model);
        Task Delete(string id);
        Task<List<LinhVuc>> Get();
        Task<LinhVuc> GetById(string id);
        Task<PagingModel<LinhVuc>> GetPaging(LinhVucParam param);
        
        
        Task<PagingModel<DonViShort>> GetByIdByFields(ChildPaging childPaging);
        Task AddUnit(List<DonViShort> model);   
        Task DeleteUnit(DonViShort model);
        
        
    }
}