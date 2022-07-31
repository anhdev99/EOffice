using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;

namespace EOffice.WebAPI.Services
{
    public class HopThuService: BaseService, IHopThuService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<HopThu, string> BaseMongoDb;
        private readonly IMongoCollection<HopThu> _collection;
        private readonly IDbSettings _settings;
        private readonly IFileService _fileService;
        private ILoggingService _logger;
        private INotifyService _notifyService;
        private readonly HistoryVanBanDiService _history;
        private List<String> filePicture = new List<string>() { ".jpeg", ".jpg", ".gif", ".png" };
        private List<String> fileOffice = new List<string>() { ".docx", ".doc", ".pdf" };

        public HopThuService(HistoryVanBanDiService history, ILoggingService logger, IDbSettings settings,
            INotifyService notifyService,
            DataContext context,
            IFileService fileService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HopThu, string>(_context.HopThu);
            _collection = context.HopThu;
            _settings = settings;
            _fileService = fileService;
            _notifyService = notifyService;
            _logger = logger.WithCollectionName(_settings.VanBanDiCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _history = history.WithUserName(CurrentUserName);
        }
        
        public async Task<HopThu> CreateSend(HopThu model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new HopThu()
            {
                TieuDe = model.TieuDe,
                NoiDung = model.NoiDung,
                NguoiNhans = model.NguoiNhans,
                Cc = model.Cc,
            };
            
             if (model.UploadFiles != default && model.UploadFiles.Count > 0)
             {
                 foreach (var file in model.UploadFiles)
                 {
                     var newFile = new FileShort();
                     newFile.FileId = file.FileId;
                     newFile.FileName = file.FileName;
                     newFile.Ext = file.Ext;
                     entity.Files.Add(newFile);
                 }
             }

             var userRep = new List<UserShort>();

             if (model.NguoiNhans != default)
             {
                 userRep.AddRange(model.NguoiNhans);
             }
             if (model.Cc != default)
             {
                 userRep.AddRange(model.Cc);
             }

             entity.NguoiGui = CurrentUserShort;
             entity.NgayGui = DateTime.Now;
             
             foreach (var item in userRep)
             {
                 var tempEmail = entity;
                 tempEmail.DaXem = false;
                 tempEmail.NgayNhan = DateTime.Now;
                 tempEmail.NguoiNhan = item;
                 
                 await BaseMongoDb.CreateAsync(tempEmail);
             }

             return entity;
        }
        
        public Task<HopThu> Create(HopThu model)
        {
            throw new NotImplementedException();
        }

        public Task<HopThu> Update(HopThu model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<PagingModel<HopThu>> GetPaging(HopThuParam param)
        {
            var result = new PagingModel<HopThu>();
            var builder = Builders<HopThu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.NguoiNhan.UserName == CurrentUserName && x.IsDeleted == false));
            
            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

    }
}