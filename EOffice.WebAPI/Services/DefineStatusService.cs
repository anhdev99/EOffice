using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.Services
{
    public class DefineStatusService : BaseService, IDefineStatusService
    {
        private DataContext _context;
        private BaseMongoDb<LinhVuc, string> BaseMongoDb;
        private IMongoCollection<LinhVuc> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        public DefineStatusService (ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            
        }
        
        public async Task<List<StatusQuestion>> GetStatusQuestion()
        {
            List<StatusQuestion> result = new List<StatusQuestion>();
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.KHONGTIEPNHAN.ToString(),
                StatusName = "Không tiếp nhận"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.CHODUYET.ToString(),
                StatusName = "Chờ duyệt"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.VUATIEPNHAN.ToString(),
                StatusName = "Vừa tiếp nhận"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.DANGXULY.ToString(),
                StatusName = "Đang xử lý"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.DAXULYXONG.ToString(),
                StatusName = "Đã xử lý xong"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.DATRALOINGUOIDAN.ToString(),
                StatusName = "Đã trả lời người dân"
            });
            return await Task.FromResult(result);
        }
    }
}