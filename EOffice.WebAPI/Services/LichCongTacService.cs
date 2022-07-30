using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Enums;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;

namespace EOffice.WebAPI.Services
{
    public class LichCongTacService : BaseService, ILichCongTacService
    {
        private DataContext _context;
        private BaseMongoDb<LichCongTac, string> BaseMongoDb;
        private IMongoCollection<LichCongTac> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public LichCongTacService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LichCongTac, string>(_context.LichCongTac);
            _collection = context.LichCongTac;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.LichCongTacCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<LichCongTac> Create(LichCongTac model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new LichCongTac
            {
                TuNgay = model.TuNgay,
                DenNgay = model.DenNgay,
                ThoiGian = model.ThoiGian,
                ChuTri = model.ChuTri,
                MauSac = model.MauSac,
                DiaDiem = model.DiaDiem,
                TieuDe = model.TieuDe,
                ThanhPhanThamDu = model.ThanhPhanThamDu,
                GhiChu = model.GhiChu,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
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

        public async Task<LichCongTac> Update(LichCongTac model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.LichCongTac.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.TuNgay = model.TuNgay;
            entity.DenNgay = model.DenNgay;
            entity.ThoiGian = model.ThoiGian;
            entity.ChuTri = model.ChuTri;
            entity.MauSac = model.MauSac;
            entity.DiaDiem = model.DiaDiem;
            entity.TieuDe = model.TieuDe;
            entity.ThanhPhanThamDu = model.ThanhPhanThamDu;
            entity.GhiChu = model.GhiChu;
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


            var entity = _context.LichCongTac.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<LichCongTac>> Get()
        {
            return await _context.LichCongTac.Find(x => x.IsDeleted != true).SortByDescending(x => x.TuNgay)
                .ToListAsync();
        }
        
        public async Task<List<LichCongTac>> GetByDateNow()
        {
            DateTime today = DateTime.Now;
            var today_fm = today.ToString("dd/MM/yyyy");
            var entity = await  _context.LichCongTac.Find(x => x.IsDeleted != true).ToListAsync();
            List<LichCongTac> list = new List<LichCongTac>();
            foreach (var item in entity)
            {
                var tungay_fm = item.TuNgay.ToString("dd/MM/yyyy");
                if (tungay_fm == today_fm)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task<LichCongTac> GetById(string id)
        {
            return await _context.LichCongTac.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<LichCongTac>> GetPaging(PagingParam param)
        {
            PagingModel<LichCongTac> result = new PagingModel<LichCongTac>();
            var builder = Builders<LichCongTac>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));

            string sortBy = nameof(LichCongTac.TuNgay);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LichCongTac>
                        .Sort.Descending(sortBy)
                    : Builders<LichCongTac>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
        public async Task<List<LichCongTac>> GetByDate(PagingParamDate param)
        {
            DateTime start = Convert.ToDateTime(param.DateRange.start);
            DateTime end = new DateTime();
            if (param.DateRange.end != "")
            {
                end = Convert.ToDateTime(param.DateRange.end);
            }
           
            String.Format("{0:d/M/yyyy HH:mm:ss}", start);
            String.Format("{0:d/M/yyyy HH:mm:ss}", end);
            var entity = await  _context.LichCongTac.Find(x => x.IsDeleted != true).ToListAsync();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }
            List<LichCongTac> list = new List<LichCongTac>();
            foreach (var item in entity)
            {
                String.Format("{0:d/M/yyyy HH:mm:ss}", item.TuNgay);
                if (param.DateRange.end == "")
                {
                    int compare = DateTime.Compare(start, item.TuNgay);
                    if (compare > 0)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    int compareStart = DateTime.Compare(start, item.TuNgay);
                    int compareEnd = DateTime.Compare(end, item.TuNgay);
                    if (compareStart <= 0 && compareEnd >= 0)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
    }
}