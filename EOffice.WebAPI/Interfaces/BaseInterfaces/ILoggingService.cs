using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.Services;

namespace EOffice.WebAPI.Interfaces
{
    public interface ILoggingService
    {
        Task<bool> SaveChanges();
        LoggingService WithCollectionName(string collectionName);
        LoggingService WithDatabaseName(string databaseName);
        LoggingService WithAction(string action);
        LoggingService WithUserName(string userName);
        LoggingService WithContentLog(string contentLog);
        LoggingService WithActionResult(string actionResult);
        
        Task<Logging> GetById(string id);
        Task<PagingModel<Logging>> GetPaging(PagingParam param);
    }
}