using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.Services;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Interfaces
{
    public interface INotifyService
    {
        NotifyService WithNotify(Notify notify);
        NotifyService WithRecipients(List<string> recipientIds);
        NotifyService WithRecipient(string recipientId);
        Task PushNotify();
        Task<PagingModel<Notify>> GetPaging(NotifyParam param);
        Task<Notify> GetById(string id);
        Task<ResultResponse<NotifyVM>> GetListNotify();
        Task<ResultResponse<Notify>> ChangeStatus(string id);
        Task LuuCVNoiBo(string idNotify);
    }
}