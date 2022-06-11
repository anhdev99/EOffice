using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IDefineStatusService
    {
        Task<List<StatusQuestion>> GetStatusQuestion();
        
    }
}