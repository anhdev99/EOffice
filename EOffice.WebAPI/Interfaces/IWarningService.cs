using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IWarningService
    {
        Task<Warning> Create(Warning model);
        Task<Warning> Update(Warning model);
        Task Delete(string id);
        Task<List<Warning>> Get();
        Task<Warning> GetById(string id);
        Task<PagingModel<Warning>> GetPaging(WarningParam param);
    }
}