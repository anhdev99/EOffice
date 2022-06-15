using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Enums;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.OpenApi.Extensions;
using StringExtensions = EOffice.WebAPI.Extensions.StringExtensions;

namespace EOffice.WebAPI.Services
{
    public class HistoryVanBanDiService : BaseService
    {
        private DataContext _context;
        private BaseMongoDb<HistoryQuestion, string> BaseMongoDb;
        private IMongoCollection<HistoryQuestion> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public HistoryVanBanDiService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HistoryQuestion, string>(_context.HistoryQuestions);
            _collection = context.HistoryQuestions;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.HistoryQuestionCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            history = new HistoryQuestion();
            history.CreatedBy = CurrentUserName;
        }

        public async Task<List<HistoryQuestion>> GetHistoryByQuestionId(string questionId)
        {
            return await _collection.Find(x => x.QuestionId == questionId).SortByDescending(x => x.ModifiedAt).ToListAsync();
        }
        public async Task<bool> SaveChangeHistoryQuestion()
        {
            if (history == default)
                return false;
            Hash();
            history.Id = BsonObjectId.GenerateNewId().ToString();
            var result = await BaseMongoDb.CreateAsync(history);
            if (result.Entity.Id == default || !result.Success)
                return false;
            return true;
        }

        public HistoryVanBanDiService WithTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                history.Title = title;
            }

            return this;
        }
        public HistoryVanBanDiService WithStatus(string trangThai, string trangThaiTen)
        {
            if (trangThai != default)
            {
                history.TrangThai = trangThai;
                history.TrangThaiTen = trangThaiTen;
            }

            return this;
        }
        public HistoryVanBanDiService WithUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = _context.Users.Find(x => x.UserName == userName).FirstOrDefault();
                if (user == default)
                    return this;

                history.UserName = user.UserName;
                history.UserFullName = user.FullName;
            }

            return this;
        }

        public HistoryVanBanDiService WithDepartment(string departmentId)
        {
            if (!string.IsNullOrEmpty(departmentId))
            {
                var entity = _context.DonVis.Find(x => x.Id == departmentId).FirstOrDefault();
                if (entity == default)
                    return this;

                history.DepartmentId = entity.Id;
                history.DepartmentName = entity.Ten;
            }

            return this;
        }
        public HistoryVanBanDiService WithQuestionId(string questionId)
        {
            if (!string.IsNullOrEmpty(questionId))
            {
                history.QuestionId = questionId;
            }

            return this;
        }
        public HistoryVanBanDiService WithType(ETypeHistory type, dynamic oldValue)
        {
            if (!string.IsNullOrEmpty(type.GetDisplayName()))
            {
                history.TypeHistory = type.GetDisplayName();
                history.OldValue = oldValue;
            }

            return this;
        }
        public HistoryVanBanDiService WithAction(EAction action)
        {
            history.Action = action.GetDisplayName();

            return this;
        }
        public HistoryVanBanDiService WithAction(string action)
        {
            history.Action = action;

            return this;
        }
        public HistoryVanBanDiService WithHistoryAction(HistoryAction action)
        {
            if (action != default)
            {
                history.HistoryAction = action;
            }

            return this;
        }

        private void Hash()
        {
            var hash = StringExtensions.SHA256(history.CreatedAt.ToString() + history.CreatedBy + history.Id);
            history.Hash = hash;
            var pre = _collection.Find(x => x.QuestionId == history.QuestionId)
                .SortByDescending(x => x.CreatedAt).FirstOrDefault();
            if (pre != default)
                history.ReferenceHash = pre.Hash;
        }
        #region Properties

        private HistoryQuestion history;

        #endregion
    }
}