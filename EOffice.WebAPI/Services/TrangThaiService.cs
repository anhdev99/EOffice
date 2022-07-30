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
using EOffice.WebAPI.ViewModels;
using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;

namespace EOffice.WebAPI.Services
{
    public class TrangThaiService : BaseService, ITrangThaiService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<TrangThai, string> BaseMongoDb;
        private readonly IMongoCollection<TrangThai> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;

        public TrangThaiService(ILoggingService logger, IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<TrangThai, string>(_context.TrangThai);
            _collection = context.TrangThai;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.WarningCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<TrangThai> Create(TrangThai model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new TrangThai()
            {
                Code = model.Code,
                ThuTu = model.ThuTu,
                Ten = model.Ten,
                Color = model.Color,
                BgColor = model.BgColor,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                Actions = model.Actions,
                NextTrangThai = model.NextTrangThai
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

        public async Task<TrangThai> Update(TrangThai model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.TrangThai.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkCodeExist = _context.TrangThai.Find(x => x.Id != model.Id && x.Code == model.Code).FirstOrDefault();
            if (checkCodeExist != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.NAME_EXIST.ToString())
                    .WithMessage("Code đã tồn tại.");
            }
            
            entity.Code = model.Code;
            entity.Ten = model.Ten;
            entity.Color = model.Color;
            entity.BgColor = model.BgColor;
            entity.ThuTu = model.ThuTu;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            entity.Actions = model.Actions;
            entity.NextTrangThai = model.NextTrangThai;

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


            var entity = _context.TrangThai.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }
        

        public async Task<TrangThai> GetById(string id)
        {
            return await _context.TrangThai.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<TrangThai>> GetPaging(TrangThaiParam param)
        {
            var result = new PagingModel<TrangThai>();
            var builder = Builders<TrangThai>.Filter;
            var filter = builder.Empty;

            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Code.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(Warning.WarningDate);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<TrangThai>
                        .Sort.Ascending(sortBy)
                    : Builders<TrangThai>
                        .Sort.Descending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<List<TrangThai>> GetAll()
        {
            return _context.TrangThai.Find(x => x.IsDeleted != true).ToList();
        }
         
        public async Task<List<TrangThaiTreeVM>> GetTree()
        {
            var listTrangThai = await _context.TrangThai.Find(x  => x.IsDeleted ==false).SortBy(x=> x.Ten).ToListAsync();
            List<TrangThaiTreeVM> list = new List<TrangThaiTreeVM>();
            foreach (var item in listTrangThai)
            {
                TrangThaiTreeVM trangThai = new TrangThaiTreeVM(item);
                list.Add(trangThai);
            }
            return list;
        }
        
        public List<TrangThaiTreeVM> GetAllTree()
        {
            var data =  _context.TrangThai.Find(x => x.IsDeleted != true)
                .ToList()
                .Select(x => new TrangThaiTreeVM(x))
                .ToList();
            return data;
        }

        public async Task<List<TrangThaiShort>> GetNextTrangThai(TrangThaiParam param)
        {
            var loaiTrangThai = await _context.LoaiTrangThai
                .Find(x => x.Code == param.LoaiTrangThai
                           && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            
            if (loaiTrangThai == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy loại trạng thái");
            }

            if (param.CurrentTrangThai != default)
            {
                var currentTrangThai = _context.TrangThai.Find(x => x.Code.ToUpper() == param.CurrentTrangThai.Code.ToUpper()).FirstOrDefault();
                if (currentTrangThai != default)
                   return currentTrangThai.NextTrangThai;
            }

            if (loaiTrangThai.ListTrangThai != default)
                return loaiTrangThai.ListTrangThai;

            return  new List<TrangThaiShort>();
        }
    }
}