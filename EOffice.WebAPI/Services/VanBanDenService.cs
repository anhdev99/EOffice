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
using EOffice.WebAPI.ViewModels;
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
                LoaiVanBan = model.LoaiVanBan,
                TrangThai = model.TrangThai,
                SoLuuCV = model.SoLuuCV,
                SoVBDen = model.SoVBDen,
                NgayNhap = model.NgayNhap,
                NgayNhan = model.NgayNhan,
                NgayBanHanh = model.NgayBanHanh,
                TrichYeu = model.TrichYeu,
                HinhThucNhan = model.HinhThucNhan,
                LinhVuc = model.LinhVuc,
                MucDoBaoMat = model.MucDoBaoMat,
                MucDoTinhChat = model.MucDoTinhChat,
                HoSoDonVi = model.HoSoDonVi,
                NoiLuuTru = model.NoiLuuTru,
                CoQuanGui = model.CoQuanGui,
                KhoiCoQuanGui = model.KhoiCoQuanGui,
                HanXuLy = model.HanXuLy,
                CongVanChiDoc = model.CongVanChiDoc,
                BanChinh = model.BanChinh,
                HienThiThongBao = model.HienThiThongBao,
                NguoiKy = model.NguoiKy,
                NgayKy = model.NgayKy,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };


            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
                var tempExt = fileOffice.Where(x => exps.Contains(x)).FirstOrDefault();
                if (tempExt == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Định dạng tệp tin không đúng.");
                }

                foreach (var item in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = item.FileId;
                    newFile.FileName = item.FileName;
                    newFile.Ext = item.Ext;
                    if (entity.File == default)
                    {
                        entity.File = new List<FileShort>();
                    }

                    entity.File.Add(newFile);
                }
            }

            
            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }
            
            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.CREATE)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithType(ETypeHistory.Question, null)
            //     .WithTitle("Thêm thành công")
            //     .SaveChangeHistoryQuestion();
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
            entity.LoaiVanBan = model.LoaiVanBan;
            entity.TrangThai = model.TrangThai;
            entity.SoLuuCV = model.SoLuuCV;
            entity.SoVBDen = model.SoVBDen;
            entity.NgayNhap = DateTime.Now;
            entity.NgayNhan = model.NgayNhan;
            entity.NgayBanHanh = model.NgayBanHanh;
            entity.TrichYeu = model.TrichYeu;
            entity.HinhThucNhan = model.HinhThucNhan;
            entity.LinhVuc = model.LinhVuc;
            entity.MucDoBaoMat = model.MucDoBaoMat;
            entity.MucDoTinhChat = model.MucDoTinhChat;
            entity.HoSoDonVi = model.HoSoDonVi;
            entity.NoiLuuTru = model.NoiLuuTru;
            entity.CoQuanGui = model.CoQuanGui;
            entity.KhoiCoQuanGui = model.KhoiCoQuanGui;
            entity.HanXuLy = model.HanXuLy;
            entity.CongVanChiDoc = model.CongVanChiDoc;
            entity.BanChinh = model.BanChinh;
            entity.HienThiThongBao = model.HienThiThongBao;
            entity.NguoiKy = model.NguoiKy;
            entity.NgayKy = model.NgayKy;
            entity.CreatedBy = CurrentUserName;
            entity.ModifiedBy = CurrentUserName;
            entity.CreatedAt = DateTime.Now;
            entity.ModifiedAt = DateTime.Now;
            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
                var tempExt = fileOffice.Where(x => exps.Contains(x)).FirstOrDefault();
                if (tempExt == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Định dạng tệp tin không đúng.");
                }

                foreach (var item in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = item.FileId;
                    newFile.FileName = item.FileName;
                    newFile.Ext = item.Ext;
                    if (entity.File == default)
                    {
                        entity.File = new List<FileShort>();
                    }

                    entity.File.Add(newFile);
                }
            }


            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.UPDATE)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithType(ETypeHistory.Question, oldValue)
            //     .WithTitle("Cập nhập thành công.")
            //     .SaveChangeHistoryQuestion();
            return entity;
        }
        public async Task<VanBanDen> ButPhe(ButPhe model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.VanBanDen.Find(x => x.Id == model.VanBanDenId).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }
            var newButPhe = new ButPhe();
            newButPhe.Id = model.Id;
            newButPhe.NoiDungButPhe = model.NoiDungButPhe;
            newButPhe.NguoiButPhe = model.NguoiButPhe;
            newButPhe.NgayButPhe = model.NgayButPhe;
            newButPhe.NguoiPhuTrach = model.NguoiPhuTrach;
            newButPhe.NguoiButPhe = model.NguoiButPhe;
            newButPhe.NguoiChuTri = model.NguoiChuTri;
            newButPhe.DonViPhoiHop = model.DonViPhoiHop;
            newButPhe.NguoiPhoiHopXuLy = model.NguoiPhoiHopXuLy;
            newButPhe.MucDoQuanTrong = model.MucDoQuanTrong;
            newButPhe.NguoiXemDeBiet = model.NguoiXemDeBiet;
            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
                var tempExt = fileOffice.Where(x => exps.Contains(x)).FirstOrDefault();
                if (tempExt == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Định dạng tệp tin không đúng.");
                }

                foreach (var item in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = item.FileId;
                    newFile.FileName = item.FileName;
                    newFile.Ext = item.Ext;
                    if (newButPhe.File == default)
                    {
                        newButPhe.File = new List<FileShort>();
                    }

                    newButPhe.File.Add(newFile);
                }
            }

            if (entity.ButPhe == default)
            {
                entity.ButPhe = new ButPhe();
            }

            entity.ButPhe = newButPhe;
            
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }
            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.UPDATE)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithType(ETypeHistory.Question, entity)
            //     .WithTitle("Bút phê thành công.")
            //     .SaveChangeHistoryQuestion();
            return entity;
        }

        public async Task<VanBanDen> PhanCong(List<PhanCong> model)
        {
            
            var entity = _collection.Find(x => x.Id == model[0].VanBanDenId).FirstOrDefault();  
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var listPhanCong = entity.PhanCong;
            if (listPhanCong != default)
            {
                foreach (var item in model)
                {
                    var tempUserExist = listPhanCong.FindIndex(x => x.NguoiNhanXuLy == item.NguoiNhanXuLy);
                    if (tempUserExist != default)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage("Đã tồn tại trong danh sách phân công");
                    }
                    entity.PhanCong.RemoveAt(tempUserExist);
                }
                
            }

            List<PhanCong> list = new List<PhanCong>();
            foreach (var item in model)
            {
                var phancong = new PhanCong();
                phancong.Id = item.Id;
                phancong.YKienChiDao = item.YKienChiDao;
                phancong.NguoiButPhe = item.NguoiButPhe;
                phancong.NguoiNhanXuLy = item.NguoiNhanXuLy;
                
                list.Add(phancong);
            }
            entity.PhanCong = list;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.UPDATE)
            //     .WithStatus(entity.TrangThai?.Ten, entity.TrangThai?.Ten)
            //     .WithType(ETypeHistory.Question, entity)
            //     .WithTitle("Thêm nhóm thực hiện thành công.")
            //     .SaveChangeHistoryQuestion();
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
            //
            // await _history.WithQuestionId(entity.Id)
            //     .WithType(ETypeHistory.Question, entity)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithAction(EAction.DELETE)
            //     .WithTitle("Xóa văn bản đi.")
            //     .SaveChangeHistoryQuestion();
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
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Id, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
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

            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.UPDATE)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithType(ETypeHistory.Question, oldValue)
            //     .WithTitle("Cập nhật văn bản đi.")
            //     .SaveChangeHistoryQuestion();
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
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Id, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
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