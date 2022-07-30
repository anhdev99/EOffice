using System;
using System.Collections.Generic;
using System.Linq;
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
                ChuTri = model.ChuTri,
                NgayXepLich = model.NgayXepLich,
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
            entity.ChuTri = model.ChuTri;
            entity.NgayXepLich = model.NgayXepLich;
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
        
        public async Task<LichCongTac> GetById(string id)
        {
            return await _context.LichCongTac.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        #region CongViec
        public async Task<LichCongTac> CreateCongViec(CongViec model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var lichCongTac = await _context.LichCongTac.Find(x => x.Id == model.LichCongTacId).FirstOrDefaultAsync();
            
            if (lichCongTac == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy lịch công tác.");
            }

            if (lichCongTac.CongViecs == default)
            {
                lichCongTac.CongViecs = new List<CongViec>();
            }

            model.Id = BsonObjectId.GenerateNewId().ToString();
            lichCongTac.CongViecs.Add(model);

            var result = await BaseMongoDb.CreateAsync(lichCongTac);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Thêm công việc không thành công!");
            }

            return lichCongTac;
        }

        public async Task<LichCongTac> UpdateCongViec(CongViec model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.LichCongTac.Find(x => x.Id == model.LichCongTacId).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var indexCongViec = entity.CongViecs.FindIndex(x => x.Id == model.Id);
            if (indexCongViec != -1)
            {
                entity.CongViecs[indexCongViec] = model;
            }
            else
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy công việc hoặc lịch công tác.");
            }
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            return entity;
        }

        public async Task DeleteCongViec(CongViec model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.LichCongTac.Find(x => x.Id == model.LichCongTacId && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var indexCongViec = entity.CongViecs.FindIndex(x => x.Id == model.Id);
            if (indexCongViec != -1)
            {
                entity.CongViecs.RemoveAt(indexCongViec);
            }
            else
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy công việc hoặc lịch công tác.");
            }
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<CongViec> GetByIdCongViec(CongViec model)
        {
            var lichCongTac = await  _context.LichCongTac.Find(x => x.Id == model.LichCongTacId && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            if (lichCongTac == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy lịch công tác!");
            }

            var congViec = lichCongTac.CongViecs.Where(x => x.Id == model.Id).FirstOrDefault();
            if (congViec == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy công việc!");
            }
            return congViec;
        }

        #endregion
    }
}