using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Enums;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.Services.SignDigital;
using EOffice.WebAPI.ViewModels;
using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;
using File = EOffice.WebAPI.Models.File;

namespace EOffice.WebAPI.Services
{
    public class VanBanDiService : BaseService, IVanBanDiService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<VanBanDi, string> BaseMongoDb;
        private readonly IMongoCollection<VanBanDi> _collection;
        private readonly IDbSettings _settings;
        private readonly IFileService _fileService;
        private ILoggingService _logger;
        private INotifyService _notifyService;
        private readonly HistoryVanBanDiService _history;
        private List<String> filePicture = new List<string>() { ".jpeg", ".jpg", ".gif", ".png" };
        private List<String> fileOffice = new List<string>() { ".docx", ".doc", ".pdf" };

        public VanBanDiService(HistoryVanBanDiService history, ILoggingService logger, IDbSettings settings,
            INotifyService notifyService,
            DataContext context,
            IFileService fileService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<VanBanDi, string>(_context.VanBanDi);
            _collection = context.VanBanDi;
            _settings = settings;
            _fileService = fileService;
            _notifyService = notifyService;
            _logger = logger.WithCollectionName(_settings.VanBanDiCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _history = history.WithUserName(CurrentUserName);
        }

        public async Task<VanBanDi> CapSoVanBan()
        {
            var vanBanDi = _context.VanBanDi.Find(x => x.IsDeleted != true).ToList();
            var identiyList = 1;
            if (vanBanDi.Count > 0)
                identiyList = vanBanDi.Max(x => x.Identity);
            var max = 0;
            if (identiyList != default)
            {
                max = ++identiyList;
            }
            else
            {
                max = 1;
            }

            var newVanBanDi = new VanBanDi();
            string formatMax = "";
            if (max < 10)
            {
                formatMax = "000" + max;
            }
            else if (max < 100)
            {
                formatMax = "00" + max;
            }
            else if (max < 1000)
            {
                formatMax = "0" + max;
            }
            else
            {
                formatMax = max.ToString();
            }

            newVanBanDi.SoLuuCV = "ĐHĐT-HC-" + formatMax + "/" + DateTime.Now.Year;
            newVanBanDi.Identity = max;
            return newVanBanDi;
        }

        public async Task<VanBanDi> Create(VanBanDi model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new VanBanDi()
            {
                Id = BsonObjectId.GenerateNewId().ToString(),
                Version = 1,
                Number = 0,
                SoLuuCV = model.SoLuuCV,
                SoVBDi = model.SoVBDi,
                Identity = model.Identity,
                CanBoSoan = model.CanBoSoan,
                DonViSoan = model.DonViSoan,
                NgayNhap = model.NgayNhap,
                NgayTraLoi = model.NgayTraLoi,
                TraLoiCVSo = model.TraLoiCVSo,
                SoBan = model.SoBan,
                TrichYeu = model.TrichYeu,
                NoiLuuTru = model.NoiLuuTru,
                NguoiKy = model.NguoiKy,
                NgayKy = model.NgayKy,
                File = model.File,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CongVanChiDoc = model.CongVanChiDoc,
                BanChinh = model.BanChinh,
                HienThiThongBao = model.HienThiThongBao,
                MucDoBaoMat = model.MucDoBaoMat,
                MucDoTinhChat = model.MucDoTinhChat,
                TrinhLanhDaoTruong = model.TrinhLanhDaoTruong,
                Ower = CurrentUserShort
            };

            if (entity.ListOwerId != default)
            {
                entity.ListOwerId.Add(CurrentUserName);
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

            entity.CoQuanNhan = model.CoQuanNhan;

            entity.KhoiCoQuanNhan = model.KhoiCoQuanNhan;
            entity.LinhVuc = model.LinhVuc;

            if (model.HoSoDonVi != default)
            {
                entity.HoSoDonVi = model.HoSoDonVi;
            }

            if (model.HinhThucGui != default)
            {
                entity.HinhThucGui = model.HinhThucGui;
            }

            if (model.LoaiVanBan != default)
            {
                entity.LoaiVanBan = model.LoaiVanBan;
            }

            if (model.TrangThai != default)
            {
                entity.TrangThai = model.TrangThai;
                if (entity.TrangThai != default &&
                    entity.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_TRUONG.ToUpper())
                {
                    entity.NhomNguoiTiepNhanVBTrinhLD = GenerateNhomNguoiTiepNhanVBTrinhLD();

                    var owerTemp = entity.NhomNguoiTiepNhanVBTrinhLD
                        .Where(x => x.RoleCode == DefaultRoleCode.VAN_THU_TRUONG).FirstOrDefault();
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

        public async Task<VanBanDi> Update(VanBanDi model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.VanBanDi.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var oldValue = entity;

            entity.SoLuuCV = model.SoLuuCV;
            entity.SoVBDi = model.SoVBDi;
            entity.CanBoSoan = model.CanBoSoan;
            entity.DonViSoan = model.DonViSoan;
            entity.NgayNhap = model.NgayNhap;
            entity.NgayTraLoi = model.NgayTraLoi;
            entity.TraLoiCVSo = model.TraLoiCVSo;
            entity.SoBan = model.SoBan;
            entity.TrichYeu = model.TrichYeu;
            entity.NoiLuuTru = model.NoiLuuTru;
            entity.NguoiKy = model.NguoiKy;
            entity.NgayKy = model.NgayKy;
            entity.CreatedBy = CurrentUserName;
            entity.Identity = model.Identity;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            entity.CongVanChiDoc = model.CongVanChiDoc;
            entity.BanChinh = model.BanChinh;
            entity.HienThiThongBao = model.HienThiThongBao;
            entity.MucDoBaoMat = model.MucDoBaoMat;
            entity.MucDoTinhChat = model.MucDoTinhChat;
            entity.File = model.File;
            entity.TrinhLanhDaoTruong = model.TrinhLanhDaoTruong;
            if (entity.ListOwerId != default)
            {
                var checkUserOwer = entity.ListOwerId.Where(x => x.ToUpper() == CurrentUserName.ToUpper())
                    .FirstOrDefault();
                if (checkUserOwer == default)
                    entity.ListOwerId.Add(CurrentUserName);
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

            entity.DonViSoan = model.DonViSoan;
            entity.CoQuanNhan = model.CoQuanNhan;
            entity.LinhVuc = model.LinhVuc;

            entity.KhoiCoQuanNhan = model.KhoiCoQuanNhan;


            if (model.HoSoDonVi != default)
            {
                entity.HoSoDonVi = model.HoSoDonVi;
            }

            if (model.HinhThucGui != default)
            {
                entity.HinhThucGui = model.HinhThucGui;
            }

            if (model.LoaiVanBan != default)
            {
                entity.LoaiVanBan = model.LoaiVanBan;
            }

            if (model.TrangThai != default)
            {
                entity.TrangThai = model.TrangThai;
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

        public async Task Delete(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.VanBanDi.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<VanBanDi>> Get()
        {
            return await _context.VanBanDi.Find(x => x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<VanBanDi> GetById(string id)
        {
            var model = await _context.VanBanDi.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            return model;
        }

        public async Task<PagingModel<VanBanDi>> GetPaging(VanBanDiParam param)
        {
            var result = new PagingModel<VanBanDi>();
            var builder = Builders<VanBanDi>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            filter = builder.And(filter,
            builder.Where(x =>
                (x.TrangThai != default &&
                 x.TrangThai.Code.ToUpper() == DefaultRoleCode.BAN_HANH) ||
                x.CreatedBy == CurrentUserName));
            // var checkQuyenThuKy =
            //     CurrentUser.Roles.Find(x =>
            //         x.Code.ToUpper() == RoleConstants.VAN_THU_TRUONG.ToUpper() ||
            //         x.Code.ToUpper() == RoleConstants.THU_KY_HIEU_TRUONG.ToUpper() ||
            //         x.Code.ToUpper() == RoleConstants.HIEU_TRUONG);
            // if (checkQuyenThuKy != default)
            // {
            //     filter = builder.And(filter,
            //         builder.Where(x =>
            //             (x.TrangThai != default &&
            //              x.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_TRUONG) ||
            //             x.CreatedBy == CurrentUserName || x.ListOwerId.Contains(CurrentUserName)));
            // }
            // else
            // {
            //     filter = builder.And(filter,
            //         builder.Where(x =>x.ListOwerId.Contains(CurrentUserName) || x.CreatedBy == CurrentUserName));
            // }

            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Code, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

               public async Task<PagingModel<VanBanDi>> GetPagingXyLy1(VanBanDiParam param)
        {
            var result = new PagingModel<VanBanDi>();
            var builder = Builders<VanBanDi>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x =>  x.IsDeleted == false));
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
                         x.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_TRUONG) ||
                        x.CreatedBy == CurrentUserName || x.ListOwerId.Contains(CurrentUserName)));
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x =>x.ListOwerId.Contains(CurrentUserName) || x.CreatedBy == CurrentUserName));
            }

            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Code, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<VanBanDi>> GetPagingXuLy(VanBanDiParam param)
        {
            var result = new PagingModel<VanBanDi>();
            var builder = Builders<VanBanDi>.Filter;
            var filter = builder.Empty;


            // var checkQuyenThuKy =
            //     CurrentUser.Roles.Find(x => x.Ten == "Thư ký hiệu trường" || x.Ten == "Văn thư trường");
            // if (checkQuyenThuKy != default)
            // {
            //     filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // }
            // else
            // {
            //     filter = builder.And(filter, builder.Where(x => x.IsDeleted == false && x.CreatedBy == CurrentUserName));
            // }

            filter = builder.And(filter, builder.Where(x => x.PhanCongKySo.Any(x => x.UserName == CurrentUserName)));

            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Code, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<VanBanDi>
                        .Sort.Descending(sortBy)
                    : Builders<VanBanDi>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<VanBanDi>> GetPagingCapSo(VanBanDiParam param)
        {
            var result = new PagingModel<VanBanDi>();
            var builder = Builders<VanBanDi>.Filter;
            var filter = builder.Empty;


            var checkQuyenThuKy =
                CurrentUser.Roles.Find(x => x.Ten == "Thư ký hiệu trường" || x.Ten == "Văn thư trường");
            if (checkQuyenThuKy != default)
            {
                filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x => x.IsDeleted == false && x.CreatedBy == CurrentUserName));
            }

            // filter = builder.And(filter, builder.Where(x => x.PhanCongKySo.Any(x => x.UserName == CurrentUserName)));

            filter = builder.And(filter, builder.Where(x => x.SoVBDi == null || x.SoVBDi == ""));

            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Code, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<VanBanDi>
                        .Sort.Descending(sortBy)
                    : Builders<VanBanDi>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<VanBanDi> PhanCong(VanBanDi model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.VanBanDi.Find(x => x.Id == model.Id).FirstOrDefault();
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
                ngpc.Add(new NguoiPhanCong() { UserId = u?.Id, Ten = u?.FullName, GhiChu = item.GhiChu });
            }

            entity.NguoiPhanCong = ngpc;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithVanBanId(entity.Id)
                .WithAction(nameof(VanBanAction.PHAN_CONG_VAN_BAN))
                .WithStatus(entity.TrangThai)
                .WithType(oldValue)
                .WithTitle(VanBanAction.PHAN_CONG_VAN_BAN)
                .SaveChangeHistory();
            return entity;
        }

        public async Task<PagingModel<VanBanDi>> GetPagingUser(VanBanDiParam param)
        {
            var result = new PagingModel<VanBanDi>();
            var builder = Builders<VanBanDi>.Filter;
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
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Code, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<VanBanDi>
                        .Sort.Descending(sortBy)
                    : Builders<VanBanDi>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            var listVB = new List<VanBanDi>();
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

        public async Task<VanBanDi> RemoveAssignSign(PhanCongKySo model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var listPhanCong = vanBanDi.PhanCongKySo;
            if (listPhanCong != default)
            {
                var tempUserExist = listPhanCong.FindIndex(x => x.UserName == model.NguoiKy?.UserName);
                if (tempUserExist < 0)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Không tồn tại trong danh sách ký số");
                }

                vanBanDi.PhanCongKySo.RemoveAt(tempUserExist);
            }

            var result = await BaseMongoDb.UpdateAsync(vanBanDi);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithVanBanId(vanBanDi.Id)
                .WithAction(nameof(VanBanAction.KY_SO_NOI_BO))
                .WithStatus(vanBanDi.TrangThai)
                .WithType(vanBanDi)
                .WithTitle("Xóa thành viên ký số: " + model.NguoiKy?.UserName)
                .SaveChangeHistory();

            return vanBanDi;
        }

        public async Task<VanBanDi> AssignSign(PhanCongKySo model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            if (model.NguoiKy == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Hãy chọn cán bộ ký số");
            }

            var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var listPhanCong = vanBanDi.PhanCongKySo;
            if (listPhanCong != default)
            {
                var tempUserExist = listPhanCong.Where(x => x.UserName == model.NguoiKy?.UserName).FirstOrDefault();
                if (tempUserExist != default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Đã tồn tại trong danh sách ký số");
                }
            }

            var newNguoiKy = new PhanCongKySo();
            newNguoiKy.Id = model.Id;
            newNguoiKy.FullName = model.NguoiKy?.FullName;
            newNguoiKy.UserName = model.NguoiKy?.UserName;
            newNguoiKy.SignImage = model.NguoiKy?.KySo;
            newNguoiKy.ChoPhepKy = model.ChoPhepKy;
            newNguoiKy.ThuTu = model.ThuTu;
            newNguoiKy.VanBanDiId = model.VanBanDiId;


            if (vanBanDi.PhanCongKySo == default)
            {
                vanBanDi.PhanCongKySo = new List<PhanCongKySo>();
            }

            vanBanDi.PhanCongKySo.Add(newNguoiKy);

            var result = await BaseMongoDb.UpdateAsync(vanBanDi);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithVanBanId(vanBanDi.Id)
                .WithAction(nameof(VanBanAction.KY_SO_NOI_BO))
                .WithStatus(vanBanDi.TrangThai)
                .WithType(vanBanDi)
                .WithTitle("Thêm thành viên ký số: " + newNguoiKy.UserName)
                .SaveChangeHistory();

            return vanBanDi;
        }

        public async Task<VanBanDi> AssignOrReject(PhanCongKySo model, string path)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var listPhanCong = vanBanDi.PhanCongKySo;
            if (listPhanCong != default)
            {
                var tempUserExist = listPhanCong.Where(x => x.UserName == CurrentUserName).FirstOrDefault();
                if (tempUserExist == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Không tồn tại trong danh sách ký số");
                }

                var user = _context.Users.Find(x => x.UserName == CurrentUserName).FirstOrDefault();

                byte[] passHash, passSalt;
                passHash = user.PasswordHash;
                passSalt = user.PasswordSalt;
                var pass = PasswordExtensions.VerifyPasswordHash(model.Password, passHash, passSalt);
                if (pass != true)
                {
                    throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Mật khẩu không chính xác");
                }

                var tempUserExistIndex = listPhanCong.FindIndex(x => x.UserName == CurrentUserName);

                if (model.Reject)
                {
                    // vanBanDi.PhanCongKySo = new List<PhanCongKySo>();
                    // await _history.WithQuestionId(vanBanDi.Id)
                    //     .WithAction(EAction.UPDATE)
                    //     .WithStatus(vanBanDi.TrangThai?.Ten, vanBanDi.TrangThai?.Ten)
                    //     .WithType(ETypeHistory.Question, vanBanDi)
                    //     .WithTitle("Đã có thành viên từ chối")
                    //     .SaveChangeHistoryQuestion();
                }

                vanBanDi.PhanCongKySo[tempUserExistIndex].NgayKy = DateTime.Now;
                vanBanDi.PhanCongKySo[tempUserExistIndex].Content = model.Content;
                vanBanDi.PhanCongKySo[tempUserExistIndex].Reject = model.Reject;
                vanBanDi.PhanCongKySo[tempUserExistIndex].NgayKyString = model.NgayKyString;
            }

            var result = await BaseMongoDb.UpdateAsync(vanBanDi);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithVanBanId(vanBanDi.Id)
                .WithAction(nameof(VanBanAction.KY_SO_NOI_BO))
                .WithStatus(vanBanDi.TrangThai)
                .WithType(vanBanDi)
                .WithTitle("Ký thành công: " + CurrentUserName)
                .SaveChangeHistory();

            var vanBanNew = _collection.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();


            if (vanBanNew != default)
            {
                var checkPhanCongKy = vanBanNew.PhanCongKySo.Where(x => x.NgayKyString == null && x.ChoPhepKy).ToList();
                if (checkPhanCongKy.Count <= 0)
                {
                 
                    var resultFile = await TienHanhKySo(vanBanNew.PhanCongKySo, path,
                        vanBanNew.File.Select(x => x.FileId).ToList());

                    if (resultFile != default)
                    {
                        foreach (var item in resultFile)
                        {
                            var newFile = new FileShort();
                            newFile.FileId = item.Id;
                            newFile.FileName = item.FileName;
                            newFile.Ext = item.Ext;
                            if (vanBanNew.FilePDF == default)
                            {
                                vanBanNew.FilePDF = new List<FileShort>();
                            }

                            vanBanNew.FilePDF.Add(newFile);
                        }

                        var newTrangThai = _context.TrangThai.AsQueryable()
                            .Where(x => x.Code.ToUpper() == DefaultRoleCode.DA_KY_SO_DUYET.ToUpper()).Select(x =>
                                new TrangThaiShort()
                                {
                                    Id = x.Id,
                                    Code = x.Code,
                                    Ten = x.Ten,
                                    BgColor = x.BgColor,
                                    Color = x.BgColor
                                }).FirstOrDefault();
                        if (newTrangThai != default)
                        {
                            vanBanNew.TrangThai = newTrangThai;
                        }

                        var resultNew = await BaseMongoDb.UpdateAsync(vanBanNew);
                        if (!result.Success)
                        {
                            throw new ResponseMessageException()
                                .WithCode(EResultResponse.FAIL.ToString())
                                .WithMessage(DefaultMessage.UPDATE_FAILURE);
                        }
                        _history.WithVanBanId(vanBanDi.Id)
                            .WithAction(nameof(VanBanAction.CHUYEN_TRANG_THAI))
                            .WithStatus(vanBanNew.TrangThai)
                            .WithType(vanBanNew)
                            .WithTitle("Ký số nội bộ thành công");
                        
                            _history.WithContent("Hoàn thành ký số nội bộ.");
                        await _history.SaveChangeHistory();
                    }
                }
            }

            return vanBanDi;
        }

        private async Task<List<File>> TienHanhKySo(List<PhanCongKySo> phanCongKySos, string rootPath,
            List<string> fileIds)
        {
            var userNameFormPhanCongKySo = phanCongKySos.Select(x => x.UserName).ToList();
            var users = _context.Users.Find(x => userNameFormPhanCongKySo.Contains(x.UserName)).ToList();
            var fileImage = users.Where(x => x.KySo != null).Select(x => x.KySo.FileId).ToList();
            var userAssign = new List<User>();
            var filesUser = _context.Files.Find(x => fileImage.Contains(x.Id)).ToList();
            foreach (var item in phanCongKySos)
            {
                var checkUser = users.Where(x => x.UserName == item.UserName && item.ChoPhepKy).FirstOrDefault();
                if (checkUser != default)
                {
                    checkUser.NgayKy = item.NgayKyString;
                    checkUser.FilePath = filesUser.Where(x => x.Id == checkUser.KySo?.FileId).FirstOrDefault()?.Path;
                    userAssign.Add(checkUser);
                }
            }

            var results = new List<File>();
            var getFileWord = _context.Files.Find(x => fileIds.Contains(x.Id)).ToList();
            foreach (var file in getFileWord)
            {
                var fileWord = file;
                var kySoFunc = new KySoNoiBoService();
                var fileName = "";
                var filePathPDF = "";
                if (fileWord.Ext == ".docx")
                {
                    filePathPDF = rootPath + "/" + fileWord.FileName.Replace(".docx", ".pdf");
                    fileName = fileWord.FileName.Replace(".docx", ".pdf");
                }
                else if (fileWord.Ext == ".doc")
                {
                    filePathPDF = rootPath + "/" + fileWord.FileName.Replace(".doc", ".pdf");
                    fileName = fileWord.FileName.Replace(".doc", ".pdf");
                }

                kySoFunc.TienTrinhKySo(fileWord.Path, fileWord.FileName, filePathPDF, userAssign);
                var resultFile =
                    await _fileService.SaveFileAsync(filePathPDF, fileName, Guid.NewGuid().ToString() + ".pdf", ".pdf",
                        100);
                if (resultFile != default)
                {
                    results.Add(resultFile);
                }
            }


            return results;
        }

        public async Task<List<PhanCongKySo>> GetPhanCongKySoByVanBanId(string vanBanId)
        {
            var vanBanDi = _context.VanBanDi.Find(x => x.Id == vanBanId).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            ;
            if (vanBanDi.PhanCongKySo != default)
                return vanBanDi.PhanCongKySo.OrderByDescending(x => x.ThuTu).ToList();
            return vanBanDi.PhanCongKySo = new List<PhanCongKySo>();
        }

        public async Task<VanBanDi> CapSoVanBanDi(VanBanDi model)
        {
            var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.Id).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            ;
            vanBanDi.SoVBDi = model.SoVBDi;
            vanBanDi.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(vanBanDi);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithVanBanId(vanBanDi.Id)
                .WithAction(nameof(VanBanAction.CAP_SO_VAN_BAN))
                .WithStatus(vanBanDi.TrangThai)
                .WithType(vanBanDi)
                .WithTitle("Cấp số văn bản đi.")
                .SaveChangeHistory();
            return vanBanDi;
        }

        public async Task XacThuc(XacMinhVM model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var listFileId = model.UploadFiles.Select(x => x.FileId).ToList();

            var files = _context.Files.AsQueryable().Where(x => listFileId.Contains(x.Id)).ToList();

            foreach (var file in files)
            {
                SignDigitalService.ValidDsign(file.Path, model.User);
            }
        }

        public async Task ChuyenTrangThaiVanBan(TrangThaiParam model)
        {
            
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy văn bản!");
            }

            vanBanDi.TrangThai = model.NewTrangThai;
            vanBanDi.NoiDungTuChoi = null;

            #region Ký số

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.DA_THIET_LAP_KY_SO)
            {
                var userIds = model.ListPhanCongKySo.Select(x => x.UserName).ToList();
                if (vanBanDi.ListOwerId == default)
                {
                    vanBanDi.ListOwerId = new List<string>();
                }
                vanBanDi.ListOwerId.AddRange(userIds);
                var users = _context.Users.Find((x => userIds.Contains(x.UserName))).ToList();
                if (vanBanDi.PhanCongKySo == default)
                {
                    vanBanDi.PhanCongKySo = new List<PhanCongKySo>();
                }
                foreach (var user in model.ListPhanCongKySo)
                {
                    var userTemp = users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                    var newNguoiKy = new PhanCongKySo();
                    newNguoiKy.Id = user.Id;
                    newNguoiKy.FullName = user?.FullName;
                    newNguoiKy.UserName = user?.UserName;
                    newNguoiKy.SignImage = userTemp?.KySo;
                    newNguoiKy.ChoPhepKy = user.ChoPhepKy;
                    newNguoiKy.ThuTu = user.ThuTu;
                    newNguoiKy.VanBanDiId = model.VanBanDiId;
                    vanBanDi.PhanCongKySo.Add(newNguoiKy);
                }

            }
            

            #endregion
            #region Ban hành

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.BAN_HANH)
            {
                var donViIds = model.DonVi?.Select(x => x.Id).ToList();
                var userIds = _context.Users.AsQueryable().Where(x => donViIds.Contains(x.DonVi.Id) && x.IsDeleted != true).Select(x => x.Id).ToList();
                vanBanDi.NguoiDuocBanHanh = userIds;
                try
                {
                    var notify = new Notify()
                    {
                        Title =
                            $"Văn bản số {vanBanDi.SoLuuCV} được ban hành!",
                        Content =
                            $"{vanBanDi.TrichYeu}",
                        LoaiCongVan = ELoaiCongVan.CONG_VAN_DI,
                        CongVanId = vanBanDi.Id
                    };
                    await  _notifyService.WithNotify(notify).WithRecipients(userIds).PushNotify();

                    
                }
                catch (Exception e)
                {
                }
            }

            #endregion
            #region Trình lãnh đạo đơn vị
            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_DON_VI)
            {
                if( vanBanDi.LanhDaoDonVi == default)
                    vanBanDi.LanhDaoDonVi = new NhomNguoiTiepNhanVBTrinhLD();
                vanBanDi.LanhDaoDonVi.NguoiXuLy = model.LanhDaoDonVi;
                vanBanDi.Ower = model.LanhDaoDonVi;
                vanBanDi.ListOwerId.Add(model.LanhDaoDonVi.UserName);
            }
            if (model.CurrentTrangThai != default &&
                model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_DON_VI && vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.DA_DUYET)
            {
                if( vanBanDi.LanhDaoDonVi == default)
                    vanBanDi.LanhDaoDonVi = new NhomNguoiTiepNhanVBTrinhLD();
                vanBanDi.LanhDaoDonVi.NguoiXuLy = model.LanhDaoDonVi;
                vanBanDi.LanhDaoDonVi.TrangThaiXuLy = model.NewTrangThai;
                vanBanDi.Ower =  _context.Users.AsQueryable().Where(x => x.UserName == vanBanDi.CreatedBy)
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
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.LANH_DAO_DON_VI_DUYET.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDi.TrangThai = trangThai;
            }
            if (model.CurrentTrangThai != default &&
                model.CurrentTrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_DON_VI && vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI)
            {
                if( vanBanDi.LanhDaoDonVi == default)
                    vanBanDi.LanhDaoDonVi = new NhomNguoiTiepNhanVBTrinhLD();
                vanBanDi.LanhDaoDonVi.NguoiXuLy = model.LanhDaoDonVi;
                vanBanDi.LanhDaoDonVi.TrangThaiXuLy = model.NewTrangThai;
                vanBanDi.Ower =  _context.Users.AsQueryable().Where(x => x.UserName == vanBanDi.CreatedBy)
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
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.LANH_DAO_DON_VI_TU_CHOI.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDi.TrangThai = trangThai;
                vanBanDi.NoiDungTuChoi = model.NoiDung;
            }
            #endregion

            if (vanBanDi.TrangThai != default && DefaultRoleCode.TrangThaiGhiNhanThongTin.Contains(  vanBanDi.TrangThai.Code.ToUpper()) &&
                vanBanDi.NhomNguoiTiepNhanVBTrinhLD != default && vanBanDi.NhomNguoiTiepNhanVBTrinhLD.Count > 0)
            {
                var nguoiTiepNhanIndex = vanBanDi.NhomNguoiTiepNhanVBTrinhLD
                    .FindIndex(x => x.NguoiXuLy != default
                                    && x.NguoiXuLy.UserName == model.UserName);
                if (nguoiTiepNhanIndex >= 0)
                {
                    vanBanDi.NhomNguoiTiepNhanVBTrinhLD[nguoiTiepNhanIndex].NoiDung = model.NoiDung;
                    vanBanDi.NhomNguoiTiepNhanVBTrinhLD[nguoiTiepNhanIndex].TrangThaiXuLy = model.NewTrangThai;
                }
                else
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Tài khoản không có quyền xét duyệt!");
                }
            }

            #region Hiệu trưởng

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.DA_DUYET.ToUpper() &&
                CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.HIEU_TRUONG))
            {
                var trangThai = _context.TrangThai.AsQueryable()
                    .Where(x => x.Code.ToUpper() == DefaultRoleCode.HIEU_TRUONG_DA_DUYET.ToUpper()).Select(x =>
                        new TrangThaiShort()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Ten = x.Ten,
                            BgColor = x.BgColor,
                            Color = x.BgColor
                        }).FirstOrDefault();
                vanBanDi.TrangThai = trangThai;
                vanBanDi.NoiDungTuChoi = model.NoiDung;
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.KY_SO_PHAP_LY.ToUpper() &&
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
                vanBanDi.TrangThai = trangThai;
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
            }

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
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
                vanBanDi.TrangThai = trangThai;
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.THU_KY_HIEU_TRUONG);
                vanBanDi.NoiDungTuChoi = model.NoiDung;
            }

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
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
                vanBanDi.TrangThai = trangThai;
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.THU_KY_HIEU_TRUONG);
                vanBanDi.NoiDungTuChoi = model.NoiDung;
            }

            #endregion

            #region Văn thư trường

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
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
                vanBanDi.TrangThai = trangThai;
                var userOwer = _context.Users.AsQueryable().Where(x => x.UserName == vanBanDi.CreatedBy)
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
                vanBanDi.Ower = userOwer;
                vanBanDi.NoiDungTuChoi = model.NoiDung;
            }

            #endregion

            #region Thư ký hiệu trưởng

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TU_CHOI.ToUpper() &&
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
                vanBanDi.TrangThai = trangThai;
                var userOwer = _context.Users.AsQueryable().Where(x => x.UserName == vanBanDi.ModifiedBy)
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
                vanBanDi.Ower = userOwer;
                vanBanDi.NoiDungTuChoi = model.NoiDung;
            }

            #endregion

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_LANH_DAO_TRUONG)
            {
                if (vanBanDi.NhomNguoiTiepNhanVBTrinhLD == default || (vanBanDi.NhomNguoiTiepNhanVBTrinhLD != default &&
                                                                       vanBanDi.NhomNguoiTiepNhanVBTrinhLD.Count <= 0))
                    vanBanDi.NhomNguoiTiepNhanVBTrinhLD = GenerateNhomNguoiTiepNhanVBTrinhLD();
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
                if (vanBanDi.ListOwerId != default && vanBanDi.Ower != default)
                {
                    vanBanDi.ListOwerId.Add(vanBanDi.Ower.UserName);
                }
            }

            if (vanBanDi.TrangThai != default &&
                vanBanDi.TrangThai.Code.ToUpper() == DefaultRoleCode.TRINH_THU_KY_HIEU_TRUONG.ToUpper())
            {
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.THU_KY_HIEU_TRUONG);
                if (vanBanDi.ListOwerId != default && vanBanDi.Ower != default)
                {
                    vanBanDi.ListOwerId.Add(vanBanDi.Ower.UserName);
                }
            }

            if (vanBanDi.TrangThai != default &&
                DefaultRoleCode.TrangThaiCapTruong.Contains(vanBanDi.TrangThai.Code.ToUpper()))
            {
                vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.HIEU_TRUONG);
                if (vanBanDi.ListOwerId != default && vanBanDi.Ower != default)
                {
                    vanBanDi.ListOwerId.Add(vanBanDi.Ower.UserName);
                }
            }


            vanBanDi.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDb.UpdateAsync(vanBanDi);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }


            _history.WithVanBanId(vanBanDi.Id)
                .WithAction(nameof(VanBanAction.CHUYEN_TRANG_THAI))
                .WithStatus(vanBanDi.TrangThai)
                .WithType(vanBanDi)
                .WithTitle(VanBanAction.CHUYEN_TRANG_THAI);

            if (!string.IsNullOrEmpty(model.NoiDung))
                _history.WithContent(model.NoiDung);
            else
                _history.WithContent(
                    $"Chuyển trạng thái từ: {model.CurrentTrangThai.Ten} sang {model.NewTrangThai.Ten}");
            await _history.SaveChangeHistory();
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

        public async Task ThietLapKySoPhapLy(SignDigitalVM model)
        {
            var entity = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (entity != default)
            {
                foreach (var item in model.SignDigitals)
                {
                    if (item.CurrentUser == null)
                    {
                        item.CurrentUser = CurrentUserShort;
                    }
                }
                entity.SignDigitals = model.SignDigitals;
                
                var newTrangThai = _context.TrangThai.AsQueryable().Where(x => x.Code.ToUpper() == DefaultRoleCode.KY_SO_PHAP_LY_THIETLAP.ToUpper())
                    .Select(
                        x =>
                            new TrangThaiShort()
                            {
                                Id = x.Id,
                                Code = x.Code,
                                Ten = x.Ten,
                                BgColor = x.BgColor,
                                Color = x.BgColor
                            }).FirstOrDefault();
                entity.TrangThai = newTrangThai;
                entity.Ower = entity.GetOwerWithRole(DefaultRoleCode.HIEU_TRUONG);
                if (entity.ListOwerId == default)
                    entity.ListOwerId = new List<string>();
                entity.ListOwerId.Add( entity.Ower .UserName);
                var result = await BaseMongoDb.UpdateAsync(entity);
                if (result.Entity.Id == default || !result.Success)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Thiết lập ký số không thành công!");
                }
                
                await _history.WithVanBanId(entity.Id)
                    .WithAction(nameof(VanBanAction.THIET_LAP_KY_SO_PHAP_LY))
                    .WithStatus(entity.TrangThai)
                    .WithType(null)
                    .WithTitle(VanBanAction.THIET_LAP_KY_SO_PHAP_LY)
                    .SaveChangeHistory();
            }
        }

        public  void KySoPhapLy(SignDigitalVM model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
          {
              throw new ResponseMessageException()
                  .WithCode(EResultResponse.FAIL.ToString())
                  .WithMessage("Tài khoản hoặc mật khẩu ký số pháp lý không đúng.");
          }
            string user =model.UserName;
            string pass = model.Password;

            var entity = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (entity == default)
            {
                new ResultMessageResponse().WithCode(EResultResponse.ERROR.ToString())
                    .WithMessage("Không tìm thấy văn bản đi.");
            }
            byte[] fileInput = null;
            string filePathTemp = "";
            var file = new FileShort();
           if(entity.FilePDF != default && entity.FilePDF.Any(x => x.Ext.Contains(".pdf")))
            {
                file = entity.FilePDF.Where(x => x.Ext.Contains(".pdf")).LastOrDefault();
                filePathTemp = _context.Files.AsQueryable().Where(x => x.Id == file.FileId).FirstOrDefault()?.Path;
            }
            else if (entity.File != default && entity.File.Any(x => x.Ext.Contains(".pdf")))
            {
                 file = entity.File.Where(x => x.Ext.Contains(".pdf")).LastOrDefault();
                 filePathTemp = _context.Files.AsQueryable().Where(x => x.Id == file.FileId).FirstOrDefault()?.Path;
            }

            fileInput=   System.IO.File.ReadAllBytes(filePathTemp);

            ResponseMessage result = SmartCA.getSignFileTemp1(user, pass, file.FileName, fileInput, model.SignDigitals.Where(x => x.Absolute == false).ToList());

            if (result.Content != null)
            {
                var uploadDirecotroy = "files/";
                

                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = Path.Combine(uploadDirecotroy, dateTime);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var newFileName = Guid.NewGuid().ToString() + "." + file.FileName.Split(".")[1];
                var relativePath = Path.Combine("", dateTime, newFileName);
                var filePath = Path.Combine(uploadDirecotroy, relativePath);

                using (System.IO.FileStream stream = System.IO.File.Create(filePath))
                {
                    System.Byte[] byteArray = result.Content as byte[];
                    stream.Write(byteArray, 0, byteArray.Length);
                }

                var result1 = _fileService.SaveFileAsync(filePath,  file.FileName, newFileName,  ".pdf",
                    result.Content.ToString().Length);

                Task.WhenAll(result1);
                var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
                if (vanBanDi != default)
                {
                    if (vanBanDi.FilePDF == default)
                        vanBanDi.FilePDF = new List<FileShort>();
                    vanBanDi.FilePDF.Add(new FileShort()
                        { Ext = result1.Result.Ext, FileId = result1.Result.Id, FileName = result1.Result.FileName, });
                    var newTrangThai = _context.TrangThai.AsQueryable()
                        .Where(x => x.Code.ToUpper() == DefaultRoleCode.HOAN_THANH_KY_SO.ToUpper()).Select(x =>
                            new TrangThaiShort()
                            {
                                Id = x.Id,
                                Code = x.Code,
                                Ten = x.Ten,
                                BgColor = x.BgColor,
                                Color = x.BgColor
                            }).FirstOrDefault();
                    vanBanDi.TrangThai = newTrangThai;
                    vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
                    if (vanBanDi.SignDigitals != default)
                    {
                        int lenSignDigitals = model.SignDigitals.Count;
                        for (int i = 0; i < lenSignDigitals; i++)
                        {
                            model.SignDigitals[i].Absolute = true;
                        }
                    }
                    vanBanDi.SignDigitals = model.SignDigitals;
                    ReplaceOneResult actionResult
                        = _context.VanBanDi.ReplaceOne(x => x.Id.Equals(vanBanDi.Id)
                            , vanBanDi
                            , new ReplaceOptions { IsUpsert = true });
                    var result2 = actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
                    if (!result2)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage("Ký số pháp lý không thành công!");
                    }

                    _history.WithVanBanId(entity.Id)
                        .WithAction(nameof(VanBanAction.KY_SO_PHAP_LY))
                        .WithStatus(entity.TrangThai)
                        .WithType(null)
                        .WithTitle(VanBanAction.KY_SO_PHAP_LY)
                        .SaveChangeHistory();
                }
            }
            
        }       
        public  void DongMocThemSo(SignDigitalVM model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
          {
              throw new ResponseMessageException()
                  .WithCode(EResultResponse.FAIL.ToString())
                  .WithMessage("Tài khoản hoặc mật khẩu ký số pháp lý không đúng.");
          }
            string user =model.UserName;
            string pass = model.Password;

            var entity = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
            if (entity == default)
            {
                new ResultMessageResponse().WithCode(EResultResponse.ERROR.ToString())
                    .WithMessage("Không tìm thấy văn bản đi.");
            }
            byte[] fileInput = null;
            string filePathTemp = "";
            var file = new FileShort();
           if(entity.FilePDF != default && entity.FilePDF.Any(x => x.Ext.Contains(".pdf") ))
            {
                file = entity.FilePDF.Where(x => x.Ext.Contains(".pdf")).LastOrDefault();
                filePathTemp = _context.Files.AsQueryable().Where(x => x.Id == file.FileId).FirstOrDefault()?.Path;
            }
            else if (entity.File != default && entity.File.Any(x => x.Ext.Contains(".pdf")))
            {
                 file = entity.File.Where(x =>x.Ext.Contains(".pdf")).LastOrDefault();
                 filePathTemp = _context.Files.AsQueryable().Where(x => x.Id == file.FileId).FirstOrDefault()?.Path;
            }

            fileInput=   System.IO.File.ReadAllBytes(filePathTemp);

            ResponseMessage result = SmartCA.getSignFileTemp1(user, pass, file.FileName, fileInput, model.SignDigitals.Where(x => x.Absolute == false).ToList());

            if (result.Content != null)
            {
                var uploadDirecotroy = "files/";
                

                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = Path.Combine(uploadDirecotroy, dateTime);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var newFileName = Guid.NewGuid().ToString() + "." + file.FileName.Split(".")[1];
                var relativePath = Path.Combine("", dateTime, newFileName);
                var filePath = Path.Combine(uploadDirecotroy, relativePath);

                using (System.IO.FileStream stream = System.IO.File.Create(filePath))
                {
                    System.Byte[] byteArray = result.Content as byte[];
                    stream.Write(byteArray, 0, byteArray.Length);
                }

                var result1 = _fileService.SaveFileAsync(filePath,  file.FileName, newFileName,  ".pdf",
                    result.Content.ToString().Length);

                Task.WhenAll(result1);
                var vanBanDi = _context.VanBanDi.Find(x => x.Id == model.VanBanDiId).FirstOrDefault();
                if (vanBanDi != default)
                {
                    if (vanBanDi.FilePDF == default)
                        vanBanDi.FilePDF = new List<FileShort>();
                    vanBanDi.FilePDF.Add(new FileShort()
                        { Ext = result1.Result.Ext, FileId = result1.Result.Id, FileName = result1.Result.FileName, });
                    var newTrangThai = _context.TrangThai.AsQueryable()
                        .Where(x => x.Code.ToUpper() =="DDM".ToUpper()).Select(x =>
                            new TrangThaiShort()
                            {
                                Id = x.Id,
                                Code = x.Code,
                                Ten = x.Ten,
                                BgColor = x.BgColor,
                                Color = x.BgColor
                            }).FirstOrDefault();
                    vanBanDi.TrangThai = newTrangThai;
                    vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
                    if (vanBanDi.SignDigitals != default)
                    {
                        int lenSignDigitals = model.SignDigitals.Count;
                        for (int i = 0; i < lenSignDigitals; i++)
                        {
                            if(!model.SignDigitals[i].Absolute)
                                model.SignDigitals[i].Absolute = true;
                        }
                    }
                    vanBanDi.SignDigitals = model.SignDigitals;
                    ReplaceOneResult actionResult
                        = _context.VanBanDi.ReplaceOne(x => x.Id.Equals(vanBanDi.Id)
                            , vanBanDi
                            , new ReplaceOptions { IsUpsert = true });
                    var result2 = actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
                    if (!result2)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage("Ký số pháp lý không thành công!");
                    }

                    _history.WithVanBanId(entity.Id)
                        .WithAction(nameof(VanBanAction.KY_SO_PHAP_LY))
                        .WithStatus(entity.TrangThai)
                        .WithType(null)
                        .WithTitle(VanBanAction.KY_SO_PHAP_LY)
                        .SaveChangeHistory();
                }
            }
            
        }
    }
}