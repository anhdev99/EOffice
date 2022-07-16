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
    public class VanBanDenService : BaseService, IVanBanDenService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<VanBanDen, string> BaseMongoDb;
        private readonly IMongoCollection<VanBanDen> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;
        private readonly HistoryVanBanDiService _history;
        private List<String> filePicture = new List<string>() {".jpeg", ".jpg", ".gif", ".png"};
        private List<String> fileOffice = new List<string>() {".docx", ".doc", ".csv", ".xlsx", ".pptx", ".pdf"};
        private IVanBanDenService _vanBanDenServiceImplementation;

        public VanBanDenService(HistoryVanBanDiService history, ILoggingService logger, IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<VanBanDen, string>(_context.VanBanDen);
            _collection = context.VanBanDen;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.VanBanDenCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _history = history;
        }

        public async Task<VanBanDen> Create(VanBanDen model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new VanBanDen()
            {
                Id = BsonObjectId.GenerateNewId().ToString(),
                Version = 1,
                Number = 0,
                SoLuuCV = model.SoLuuCV,
                SoVBDen = model.SoVBDen,
                NgayNhap = model.NgayNhap,
                NgayTraLoi = model.NgayTraLoi,
                TraLoiCVSo = model.TraLoiCVSo,
                SoBan = model.SoBan,
                TrichYeu = model.TrichYeu,
                NoiLuuTru = model.NoiLuuTru,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };


            if (model.UploadFiles != default)
            {
                var newFile = new FileShort();
                newFile.FileId = model.UploadFiles.FileId;
                newFile.FileName = model.UploadFiles.FileName;
                newFile.Ext = model.UploadFiles.Ext;
                entity.File = newFile;
            }

            var donVi = _context.DonVis.Find(x => x.IsDeleted != true).ToList();
            entity.DonViSoanTen = donVi.Find(x => x.Id == model.DonViSoan)?.Ten;
            entity.DonViSoan = model.DonViSoan;

            entity.CoQuanNhanTen = donVi.Find(x => x.Id == model.CoQuanNhan)?.Ten;
            entity.CoQuanNhan = model.CoQuanNhan;

            entity.KhoiCoQuanNhanTen = donVi.Find(x => x.Id == model.KhoiCoQuanNhan)?.Ten;
            entity.KhoiCoQuanNhan = model.KhoiCoQuanNhan;

            var canBo = _context.Users.Find(x => x.Id == model.CanBoSoan).FirstOrDefault();
            entity.CanBoSoan = model.CanBoSoan;
            entity.CanBoSoanTen = canBo?.UserName + "-" + canBo?.FullName;

            if (model.HoSoDonVi != default)
            {
                var hoSoDonVi = _context.HoSoDonVi.Find(x => x.Id == model.HoSoDonVi).FirstOrDefault();
                entity.HoSoDonVi = model.HoSoDonVi;
                entity.HoSoDonViTen = hoSoDonVi?.Ten;
            }

            // if (model.HinhThucGui != default)
            // {
            //     var hinhThucGui = _context.HinhThucGui.Find(x => x.Id == model.HinhThucGui).FirstOrDefault();
            //     entity.HinhThucGui = model.HinhThucGui;
            //     entity.HinhThucGuiTen = hinhThucGui?.Ten;
            // }

            if (model.LoaiVanBan != default)
            {
                var loaiVanBan = _context.LoaiVanBan.Find(x => x.Id == model.LoaiVanBan.Id).FirstOrDefault();
                entity.LoaiVanBan = model.LoaiVanBan;
                entity.LoaiVanBanTen = loaiVanBan?.Ten;
            }

            if (model.TrangThai != default)
            {
                var trangThai = _context.TrangThai.Find(x => x.Id == model.TrangThai).FirstOrDefault();
                entity.TrangThai = model.TrangThai;
                entity.TrangThaiTen = trangThai?.Ten;
            }

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }


            await _history.WithQuestionId(entity.Id)
                .WithAction(EAction.CREATE)
                .WithStatus(entity.TrangThai, entity.TrangThaiTen)
                .WithType(ETypeHistory.Question, null)
                .WithTitle("Tạo văn bản đến")
                .SaveChangeHistoryQuestion();
            return entity;
        }

        public async Task<VanBanDen> Update(VanBanDen model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.VanBanDen.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var oldValue = entity;
            entity.Version = model.Version;
            entity.Number = model.Number;
            entity.SoLuuCV = model.SoLuuCV;
            entity.SoVBDen = model.SoVBDen;
            entity.NgayNhap = model.NgayNhap;
            entity.NgayTraLoi = model.NgayTraLoi;
            entity.TraLoiCVSo = model.TraLoiCVSo;
            entity.SoBan = model.SoBan;
            entity.TrichYeu = model.TrichYeu;
            entity.NoiLuuTru = model.NoiLuuTru;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;


            if (model.UploadFiles != default)
            {
                var newFile = new FileShort();
                newFile.FileId = model.UploadFiles.FileId;
                newFile.FileName = model.UploadFiles.FileName;
                newFile.Ext = model.UploadFiles.Ext;
                entity.File = newFile;
            }

            var donVi = _context.DonVis.Find(x => x.IsDeleted != true).ToList();
            entity.DonViSoanTen = donVi.Find(x => x.Id == model.DonViSoan)?.Ten;
            entity.DonViSoan = model.DonViSoan;

            entity.CoQuanNhanTen = donVi.Find(x => x.Id == model.CoQuanNhan)?.Ten;
            entity.CoQuanNhan = model.CoQuanNhan;

            entity.KhoiCoQuanNhanTen = donVi.Find(x => x.Id == model.KhoiCoQuanNhan)?.Ten;
            entity.KhoiCoQuanNhan = model.KhoiCoQuanNhan;

            var canBo = _context.Users.Find(x => x.Id == model.CanBoSoan).FirstOrDefault();
            entity.CanBoSoan = model.CanBoSoan;
            entity.CanBoSoanTen = canBo?.UserName + "-" + canBo?.FullName;

            if (model.HoSoDonVi != default)
            {
                var hoSoDonVi = _context.HoSoDonVi.Find(x => x.Id == model.HoSoDonVi).FirstOrDefault();
                entity.HoSoDonVi = model.HoSoDonVi;
                entity.HoSoDonViTen = hoSoDonVi?.Ten;
            }

            // if (model.HinhThucGui != default)
            // {
            //     var hinhThucGui = _context.HinhThucGui.Find(x => x.Id == model.HinhThucGui).FirstOrDefault();
            //     entity.HinhThucGui = model.HinhThucGui;
            //     entity.HinhThucGuiTen = hinhThucGui?.Ten;
            // }

            if (model.LoaiVanBan != default)
            {
                var loaiVanBan = _context.LoaiVanBan.Find(x => x.Id == model.LoaiVanBan.Id).FirstOrDefault();
                entity.LoaiVanBan = model.LoaiVanBan;
                entity.LoaiVanBanTen = loaiVanBan?.Ten;
            }

            if (model.TrangThai != default)
            {
                var trangThai = _context.TrangThai.Find(x => x.Id == model.TrangThai).FirstOrDefault();
                entity.TrangThai = model.TrangThai;
                entity.TrangThaiTen = trangThai?.Ten;
            }


            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithQuestionId(entity.Id)
                .WithAction(EAction.UPDATE)
                .WithStatus(entity.TrangThai, entity.TrangThaiTen)
                .WithType(ETypeHistory.Question, oldValue)
                .WithTitle("Cập nhật văn bản đi.")
                .SaveChangeHistoryQuestion();
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


            var entity = _context.VanBanDen.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var result = await BaseMongoDb.DeleteByIdsync(id);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }

            await _history.WithQuestionId(entity.Id)
                .WithType(ETypeHistory.Question, entity)
                .WithStatus(entity.TrangThai, entity.TrangThaiTen)
                .WithAction(EAction.DELETE)
                .WithTitle("Xóa văn bản đi.")
                .SaveChangeHistoryQuestion();
        }

        public async Task<List<VanBanDen>> Get()
        {
            return await _context.VanBanDen.Find(x => x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<VanBanDen> GetById(string id)
        {
            var model = await _context.VanBanDen.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            return model;
        }
        public async Task<PagingModel<VanBanDen>> GetPaging(VanBanDenParam param)
        {
            var result = new PagingModel<VanBanDen>();
            var builder = Builders<VanBanDen>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc, param.LinhVuc));
            }

            string sortBy = nameof(VanBanDen.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<VanBanDen>
                        .Sort.Descending(sortBy)
                    : Builders<VanBanDen>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
        
        public async Task<VanBanDen> PhanCong(VanBanDen model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.VanBanDen.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var oldValue = entity;
            var user = _context.Users.Find(x => x.IsDeleted != true).ToList();
            var ngpc = new List<NguoiPhanCong>();
            foreach (var item in model.NguoiPhanCong)
            {
                var u = user.Where(x => x.Id == item.UserId).FirstOrDefault();
                ngpc.Add(new NguoiPhanCong(){UserId = u?.Id, Ten = u?.FullName, GhiChu = item.GhiChu});
            }
            entity.NguoiPhanCong = ngpc;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithQuestionId(entity.Id)
                .WithAction(EAction.UPDATE)
                .WithStatus(entity.TrangThai, entity.TrangThaiTen)
                .WithType(ETypeHistory.Question, oldValue)
                .WithTitle("Cập nhật văn bản đi.")
                .SaveChangeHistoryQuestion();
            return entity;
        }
        
        public async Task<PagingModel<VanBanDen>> GetPagingUser(VanBanDenParam param)
        {
            var result = new PagingModel<VanBanDen>();
            var builder = Builders<VanBanDen>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc, param.LinhVuc));
            }
            
            string sortBy = nameof(VanBanDen.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<VanBanDen>
                        .Sort.Descending(sortBy)
                    : Builders<VanBanDen>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            var listVB = new List<VanBanDen>();
            foreach (var item in data)
            {
                var nguoiPC = item.NguoiPhanCong;
                if (nguoiPC != default)
                {
                    var check = nguoiPC.Where(x => x.UserId == CurrentUser.Id).FirstOrDefault();
                    if (check != default)
                    {
                        listVB.Add(item);
                        continue;
                    }
                }
            }

            result.Data = listVB;
            return result;
        }
    }
}