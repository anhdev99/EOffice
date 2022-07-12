using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;

namespace EOffice.WebAPI.Services
{

    public class KhoiCoQuanService : BaseService, IKhoiCoQuanService
    {
        private DataContext _context;
        private BaseMongoDb<KhoiCoQuan, string> BaseMongoDb;
        private IMongoCollection<KhoiCoQuan> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public KhoiCoQuanService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<KhoiCoQuan, string>(_context.KhoiCoQuan);
            _collection = context.KhoiCoQuan;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.KhoiCoQuanCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }
        
                public async Task<KhoiCoQuan> Create(KhoiCoQuan model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var checkName = _context.CoQuan.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }
            var entity = new KhoiCoQuan
            {
                Ten = model.Ten,
                MoTa = model.MoTa,
                ThuTu = model.ThuTu,
                Code = model.Code,
                // DonVis = model.DonVis,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            return entity;
        }
        public async Task<KhoiCoQuan> Update(KhoiCoQuan model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.KhoiCoQuan.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.KhoiCoQuan.Find(x => x.Id != model.Id
                                                          && x.Ten.ToLower() == model.Ten.ToLower()
                                                          && x.IsDeleted != true
            ).FirstOrDefault();
            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            entity.Ten = model.Ten;
            entity.MoTa = model.MoTa;
            entity.ThuTu = model.ThuTu;
            entity.Code = model.Code;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            return entity;
        }

        public async Task Delete(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.KhoiCoQuan.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            entity.IsDeleted = true;
            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<List<KhoiCoQuan>> Get()
        {
            return await _context.KhoiCoQuan.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<KhoiCoQuan> GetById(string id)
        {
            return await _context.KhoiCoQuan.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<KhoiCoQuan>> GetPaging(PagingParam param)
        {
            PagingModel<KhoiCoQuan> result = new PagingModel<KhoiCoQuan>();
            var builder = Builders<KhoiCoQuan>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }
            string sortBy = nameof(KhoiCoQuan.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<KhoiCoQuan>
                        .Sort.Descending(sortBy)
                    : Builders<KhoiCoQuan>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
    }
}