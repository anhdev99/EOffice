using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace EOffice.WebAPI.Services
{
    public class EnumService : BaseService, IEnumService
    {
        private DataContext _context;
        private BaseMongoDb<LinhVuc, string> BaseMongoDb;
        private IMongoCollection<LinhVuc> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        public EnumService (ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            
        }

        public async Task<List<EnumModel>> GetMucDo()
        {
            List<EnumModel> result = new List<EnumModel>();
            result.Add(new EnumModel
            {
                Code = EMucDo.THAP.ToString(),
                Name = "Thấp"
            });
            result.Add(new EnumModel
            {
                Code = EMucDo.TRUNGBINH.ToString(),
                Name = "Trung bình"
            });
            result.Add(new EnumModel
            {
                Code = EMucDo.CAO.ToString(),
                Name = "Cao"
            });
            return await Task.FromResult(result);
        }
    }
}