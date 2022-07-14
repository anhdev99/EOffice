using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.Interfaces
{
    public interface IEnumService
    {
        Task<List<EnumModel>> GetMucDo();
    }
}