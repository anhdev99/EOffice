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
        private INotifyService _notifyService;

        public VanBanDenService(HistoryVanBanDiService history, ILoggingService logger, IDbSettings settings,
            DataContext context,
            INotifyService notifyService,
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
            _notifyService = notifyService;
            _history = history.WithUserName(CurrentUserName);
        }
        
        public List<NhomNguoiTiepNhanVBTrinhLD> GenerateNhomNguoiTiepNhanVBTrinhLD()
        {
            var list = new List<NhomNguoiTiepNhanVBTrinhLD>();

            var vanThuTruong = _context.Users.AsQueryable()
                .Where(x => x.Roles.Any(p => p.Code.ToUpper() == DefaultRoleCode.VAN_THU_TRUONG.ToUpper()) &&
                            x.IsDeleted != true).Select(x => new UserShort()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    DonVi = x.DonVi,
                    ChucVu = x.ChucVu,
                }).FirstOrDefault();
            var thuKyHieuTruong = _context.Users.AsQueryable().Where(x =>
                x.Roles.Any(p => p.Code.ToUpper() == DefaultRoleCode.THU_KY_HIEU_TRUONG.ToUpper()) &&
                x.IsDeleted != true).Select(x => new UserShort()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                DonVi = x.DonVi,
                ChucVu = x.ChucVu,
            }).FirstOrDefault();
            var hieuTruong = _context.Users.AsQueryable()
                .Where(x => x.Roles.Any(p => p.Code.ToUpper() == DefaultRoleCode.HIEU_TRUONG.ToUpper()) &&
                            x.IsDeleted != true).Select(x => new UserShort()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    DonVi = x.DonVi,
                    ChucVu = x.ChucVu,
                }).FirstOrDefault();

            for (int i = 0; i < 3; i++)
            {
                var temp = new NhomNguoiTiepNhanVBTrinhLD();
                if (i == 0)
                {
                    temp.NguoiXuLy = vanThuTruong;
                    temp.ThuTu = i;
                    temp.RoleCode = DefaultRoleCode.VAN_THU_TRUONG;
                }

                if (i == 1)
                {
                    temp.NguoiXuLy = thuKyHieuTruong;
                    temp.ThuTu = i;
                    temp.RoleCode = DefaultRoleCode.THU_KY_HIEU_TRUONG;
                }

                if (i == 2)
                {
                    temp.NguoiXuLy = hieuTruong;
                    temp.ThuTu = i;
                    temp.RoleCode = DefaultRoleCode.HIEU_TRUONG;
                }

                list.Add(temp);
            }

            return list;
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
                TrangThai = model.TrangThai,
                SoLuuCV = model.SoLuuCV,
                SoVBDen = model.SoVBDen,
                NgayNhap = model.NgayNhap,
                NgayNhan = model.NgayNhan,
                NgayBanHanh = model.NgayBanHanh,
                TrichYeu = model.TrichYeu,
                LinhVuc = model.LinhVuc,
                MucDoBaoMat = model.MucDoBaoMat,
                MucDoTinhChat = model.MucDoTinhChat,
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
                ModifiedAt = DateTime.Now,
                TrinhLanhDaoTruong = model.TrinhLanhDaoTruong,
                Ower = CurrentUserShort,
            };
            if (model.HoSoDonVi != default)
            {
                entity.HoSoDonVi = model.HoSoDonVi; 
            }

            if (model.HinhThucNhan != default)
            {
                entity.HinhThucNhan = model.HinhThucNhan;
            }

            if (model.LoaiVanBan != default)
            {
                entity.LoaiVanBan = model.LoaiVanBan;
            }
            
            if (model.TrangThai != default)
            {
                entity.TrangThai = model.TrangThai;
                if (entity.TrangThai != default &&
                    entity.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_THU_THU_KY_HIEU_TRUONG_VAN_BAN_DEN.ToUpper())
                {
                    entity.NhomNguoiTiepNhanVBTrinhLD = GenerateNhomNguoiTiepNhanVBTrinhLD();

                    var owerTemp = entity.NhomNguoiTiepNhanVBTrinhLD
                        .Where(x => x.RoleCode == DefaultRoleCode.THU_KY_HIEU_TRUONG).FirstOrDefault();
                    if (owerTemp != default)
                        entity.Ower = owerTemp.NguoiXuLy;

                    if (entity.NhomNguoiTiepNhanVBTrinhLD != default)
                    {
                        var listOwerUserNameTemp =
                            entity.NhomNguoiTiepNhanVBTrinhLD.Select(x => x.NguoiXuLy.UserName).ToList();
                        entity.ListOwerId.AddRange(listOwerUserNameTemp);
                    }
                }
            }
            
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
            
            await _history.WithVanBanId(entity.Id)
                .WithAction(nameof(VanBanAction.CREATE))
                .WithStatus(entity.TrangThai)
                .WithType(null)
                .WithTitle(VanBanAction.CREATE)
                .SaveChangeHistory();
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

            await _history.WithVanBanId(entity.Id)
                .WithAction(nameof(VanBanAction.UPDATE))
                .WithStatus(entity.TrangThai)
                .WithType(oldValue)
                .WithTitle(VanBanAction.UPDATE)
                .SaveChangeHistory();
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
            await _history.WithVanBanId(entity.Id)
                .WithType(entity)
                .WithStatus(entity.TrangThai)
                .WithAction(nameof(VanBanAction.BUP_PHE))
                .WithTitle(VanBanAction.BUP_PHE)
                .SaveChangeHistory();
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
            await _history.WithVanBanId(entity.Id)
                .WithType(entity)
                .WithStatus(entity.TrangThai)
                .WithAction(nameof(VanBanAction.DELETE))
                .WithTitle(VanBanAction.DELETE)
                .SaveChangeHistory();
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
            var checkQuyenThuKy =
                CurrentUser.Roles.Find(x =>
                    x.Code.ToUpper() == RoleConstants.VAN_THU_TRUONG.ToUpper() ||
                    x.Code.ToUpper() == RoleConstants.THU_KY_HIEU_TRUONG.ToUpper() ||
                    x.Code.ToUpper() == RoleConstants.HIEU_TRUONG);
            if (checkQuyenThuKy != default)
            {
                filter = builder.And(filter,
                    builder.Where(x =>
                        (x.TrangThai != default &&
                         x.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_VAN_BAN_DEN) ||
                        x.CreatedBy == CurrentUserName || x.ListOwerId.Contains(CurrentUserName)));
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x =>x.ListOwerId.Contains(CurrentUserName) || x.CreatedBy == CurrentUserName));
            }
            
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
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
        
        public async Task<PagingModel<VanBanDen>> GetPagingXuLy(VanBanDenParam param)
        {
            var result = new PagingModel<VanBanDen>();
            var builder = Builders<VanBanDen>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            var checkQuyenThuKy =
                CurrentUser.Roles.Find(x =>
                    x.Code.ToUpper() == RoleConstants.VAN_THU_TRUONG.ToUpper() ||
                    x.Code.ToUpper() == RoleConstants.THU_KY_HIEU_TRUONG.ToUpper() ||
                    x.Code.ToUpper() == RoleConstants.HIEU_TRUONG);
            if (checkQuyenThuKy != default)
            {
                filter = builder.And(filter,
                    builder.Where(x =>
                        (x.TrangThai != default &&
                         x.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_VAN_BAN_DEN) ||
                        x.CreatedBy == CurrentUserName || x.ListOwerId.Contains(CurrentUserName)));
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x =>x.ButPhe != null && x.ButPhe.NguoiChuTri != default && x.ButPhe.NguoiChuTri.UserName == CurrentUserName));
            }
            
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
                .SortByDescending(x => x.ModifiedAt)
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

        public async Task ChuyenTrangThaiVanBan(TrangThaiParam model)
        {
            
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var vanBanDen = _context.VanBanDen.Find(x => x.Id == model.VanBanDenId).FirstOrDefault();
            if (vanBanDen == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy văn bản!");
            }

            vanBanDen.TrangThai = model.NewTrangThai;
            vanBanDen.NoiDungTuChoi = null;
            #region Ban hành

            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.BAN_HANH_VAN_BAN_DEN)
            {
                var donViIds = model.DonVi?.Select(x => x.Id).ToList();
                var userIds = _context.Users.AsQueryable().Where(x => donViIds.Contains(x.DonVi.Id) && x.IsDeleted != true).Select(x => x.Id).ToList();
                vanBanDen.NguoiDuocBanHanh = userIds;
                try
                {
                    var notify = new Notify()
                    {
                        Title =
                            $"Văn bản số {vanBanDen.SoLuuCV} được ban hành!",
                        Content =
                            $"{vanBanDen.TrichYeu}",
                        Url = vanBanDen.Id,
                        LoaiCongVan = ELoaiCongVan.CONG_VAN_DEN,
                        CongVanId = vanBanDen.Id
                    };
                    await  _notifyService.WithNotify(notify).WithRecipients(userIds).PushNotify();

                }
                catch (Exception e)
                {
                }
            }

            #endregion

            if (vanBanDen.TrangThai != default && DefaultRoleCode.TrangThaiGhiNhanThongTin.Contains(  vanBanDen.TrangThai.Code.ToUpper()) &&
                vanBanDen.NhomNguoiTiepNhanVBTrinhLD != default && vanBanDen.NhomNguoiTiepNhanVBTrinhLD.Count > 0)
            {
                var nguoiTiepNhanIndex = vanBanDen.NhomNguoiTiepNhanVBTrinhLD
                    .FindIndex(x => x.NguoiXuLy != default
                                    && x.NguoiXuLy.UserName == model.UserName);
                if (nguoiTiepNhanIndex >= 0)
                {
                    vanBanDen.NhomNguoiTiepNhanVBTrinhLD[nguoiTiepNhanIndex].NoiDung = model.NoiDung;
                    vanBanDen.NhomNguoiTiepNhanVBTrinhLD[nguoiTiepNhanIndex].TrangThaiXuLy = model.NewTrangThai;
                }
                else
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Tài khoản không có quyền xét duyệt!");
                }
            }

            #region Hiệu trưởng
            


            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.KY_SO_PHAP_LY.ToUpper() &&
                CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.HIEU_TRUONG))
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.HIEU_TRUONG_DA_KY.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }

            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
                model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.DUYET_VAN_BAN_PHAP_LY.ToUpper() &&
                CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.HIEU_TRUONG))
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code == DefaultRoleCode.HIEU_TRUONG_TU_CHOI_DUYET).Select(x => new TrangThaiShort()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Ten = x.Ten,
                        BgColor = x.BgColor,
                        Color = x.BgColor
                    }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.THU_KY_HIEU_TRUONG);
                vanBanDen.NoiDungTuChoi = model.NoiDung;
            }

            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
                model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.KY_SO_PHAP_LY.ToUpper() &&
                CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.HIEU_TRUONG))
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code == DefaultRoleCode.HIEU_TRUONG_TU_CHOI_DUYET).Select(x => new TrangThaiShort()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Ten = x.Ten,
                        BgColor = x.BgColor,
                        Color = x.BgColor
                    }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.THU_KY_HIEU_TRUONG);
                vanBanDen.NoiDungTuChoi = model.NoiDung;
            }

            #endregion

            #region Văn thư trường

            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
                ( model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_TRUONG.ToUpper() ||
                  model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.THU_KY_HIEU_TRUONG_TU_CHOI_DUYET.ToUpper()
                ) &&
                CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.VAN_THU_TRUONG))
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code == DefaultRoleCode.VAN_THU_TRUONG_TU_CHOI_DUYET).Select(x => new TrangThaiShort()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Ten = x.Ten,
                        BgColor = x.BgColor,
                        Color = x.BgColor
                    }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                var userOwer = _context.Users.AsQueryable().Where(x => x.UserName == vanBanDen.CreatedBy)
                    .Select(x => new UserShort()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        FullName = x.FullName,
                        DonVi = x.DonVi,
                        ChucVu = x.ChucVu,
                        Avatar = x.Avatar,
                        Note = x.Note,
                        PhoneNumber = x.PhoneNumber,
                        Email = x.Email,
                    })
                    .FirstOrDefault();
                vanBanDen.Ower = userOwer;
                vanBanDen.NoiDungTuChoi = model.NoiDung;
            }

            #endregion

            #region Thư ký hiệu trưởng

            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
                model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_THU_KY_HIEU_TRUONG.ToUpper() &&
                CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.THU_KY_HIEU_TRUONG))
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code == DefaultRoleCode.THU_KY_HIEU_TRUONG_TU_CHOI_DUYET).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                var userOwer = _context.Users.AsQueryable().Where(x => x.UserName == vanBanDen.ModifiedBy)
                    .Select(x => new UserShort()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        FullName = x.FullName,
                        DonVi = x.DonVi,
                        ChucVu = x.ChucVu,
                        Avatar = x.Avatar,
                        Note = x.Note,
                        PhoneNumber = x.PhoneNumber,
                        Email = x.Email,
                    })
                    .FirstOrDefault();
                vanBanDen.Ower = userOwer;
                vanBanDen.NoiDungTuChoi = model.NoiDung;
            }

            #endregion
            

            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_THU_THU_KY_HIEU_TRUONG_VAN_BAN_DEN.ToUpper())
            {
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.THU_KY_HIEU_TRUONG);
                if (vanBanDen.ListOwerId != default && vanBanDen.Ower != default)
                {
                    vanBanDen.ListOwerId.Add(vanBanDen.Ower.UserName);
                }
            }

            if (vanBanDen.TrangThai != default &&
                DefaultRoleCode.TrangThaiCapTruong.Contains(vanBanDen.TrangThai.Code.ToUpper()))
            {
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.HIEU_TRUONG);
                if (vanBanDen.ListOwerId != default && vanBanDen.Ower != default)
                {
                    vanBanDen.ListOwerId.Add(vanBanDen.Ower.UserName);
                }
            }
            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.DA_DUYET_VAN_BAN_DEN.ToUpper())
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.DA_DUYET_VAN_BAN_DEN.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.NoiDungTuChoi = model.NoiDung;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }
            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI_VAN_BAN_DEN.ToUpper())
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.TU_CHOI_VAN_BAN_DEN.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.NoiDungTuChoi = model.NoiDung;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }
            
            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.HTXL_VAN_BAN_DEN.ToUpper())
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.HTXL_VAN_BAN_DEN.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.NoiDungTuChoi = model.NoiDung;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }
            
            if (vanBanDen.TrangThai != default &&
                vanBanDen.TrangThai.Code.ToUpper() == DefaultRoleCode.KHT_VAN_BAN_DEN.ToUpper())
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.KHT_VAN_BAN_DEN.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDen.TrangThai = trangThai;
                vanBanDen.NoiDungTuChoi = model.NoiDung;
                vanBanDen.Ower = vanBanDen.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }
            vanBanDen.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDb.UpdateAsync(vanBanDen);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }


            _history.WithVanBanId(vanBanDen.Id)
                .WithAction(nameof(VanBanAction.CHUYEN_TRANG_THAI))
                .WithStatus(vanBanDen.TrangThai)
                .WithType(vanBanDen)
                .WithTitle(VanBanAction.CHUYEN_TRANG_THAI);
            
            if (!string.IsNullOrEmpty(model.NoiDung))
                _history.WithContent(model.NoiDung);
            else
                _history.WithContent(
                    $"Chuyển trạng thái từ: {model.CurrentTrangThai.Ten} sang {model.NewTrangThai.Ten}");
            await _history.SaveChangeHistory();
        }
    }
}