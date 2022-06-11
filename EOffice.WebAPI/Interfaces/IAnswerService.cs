using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;

namespace EOffice.WebAPI.Interfaces
{
    public interface IAnswerService
    {
        Task<Answer> Create(Answer model);
        Task<Answer> Update(Answer model);
        Task Delete(string id);
        Task<List<Answer>> Get(string questionId);
        Task<Answer> GetByIdQuestion(string id);
        Task<PagingModel<Answer>> GetPaging(AnswerParam param);
    }
}