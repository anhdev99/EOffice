using System;
using System.Collections.Generic;
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
using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;

namespace EOffice.WebAPI.Services
{
    public class VanBanDiService : BaseService, IVanBanDiService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<VanBanDi, string> BaseMongoDb;
        private readonly IMongoCollection<VanBanDi> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;
        private readonly HistoryVanBanDiService _history;
        private List<String> filePicture = new List<string>() {".jpeg", ".jpg", ".gif", ".png"};
        private List<String> fileOffice = new List<string>() {".docx", ".doc", ".csv", ".xlsx", ".pptx", ".pdf"};

        public VanBanDiService(HistoryVanBanDiService history, ILoggingService logger, IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<VanBanDi, string>(_context.VanBanDi);
            _collection = context.VanBanDi;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.VanBanDiCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _history = history;
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
                MucDoTinhChat = model.MucDoTinhChat
            };


            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
                var tempExt =  fileOffice.Where(x => exps.Contains(x)).FirstOrDefault();
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
                .WithStatus(entity.TrangThai?.Ten, entity.TrangThai?.Ten)
                .WithType(ETypeHistory.Question, null)
                .WithTitle("Tạo văn bản đến")
                .SaveChangeHistoryQuestion();
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
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            entity.CongVanChiDoc = model.CongVanChiDoc;
            entity.BanChinh = model.BanChinh;
            entity.HienThiThongBao = model.HienThiThongBao;
            entity.MucDoBaoMat = model.MucDoBaoMat;
            entity.MucDoTinhChat = model.MucDoTinhChat;
            entity.File = model.File;


            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
               var tempExt =  fileOffice.Where(x => exps.Contains(x)).FirstOrDefault();
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

            await _history.WithQuestionId(entity.Id)
                .WithAction(EAction.UPDATE)
                .WithStatus(entity.TrangThai?.Ten, entity.TrangThai?.Ten)
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

            await _history.WithQuestionId(entity.Id)
                .WithType(ETypeHistory.Question, entity)
                .WithStatus(entity.TrangThai?.Ten, entity.TrangThai?.Ten)
                .WithAction(EAction.DELETE)
                .WithTitle("Xóa văn bản đi.")
                .SaveChangeHistoryQuestion();
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
                .WithStatus(entity.TrangThai.Ten, entity.TrangThai?.Ten)
                .WithType(ETypeHistory.Question, oldValue)
                .WithTitle("Cập nhật văn bản đi.")
                .SaveChangeHistoryQuestion();
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

            await _history.WithQuestionId(vanBanDi.Id)
                .WithAction(EAction.UPDATE)
                .WithStatus(vanBanDi.TrangThai?.Ten, vanBanDi.TrangThai?.Ten)
                .WithType(ETypeHistory.Question, vanBanDi)
                .WithTitle("Xóa thành viên ký số")
                .SaveChangeHistoryQuestion();

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

            await _history.WithQuestionId(vanBanDi.Id)
                .WithAction(EAction.UPDATE)
                .WithStatus(vanBanDi.TrangThai?.Ten, vanBanDi.TrangThai?.Ten)
                .WithType(ETypeHistory.Question, vanBanDi)
                .WithTitle("Thêm thành viên ký số: " + newNguoiKy.UserName )
                .SaveChangeHistoryQuestion();

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
                    await _history.WithQuestionId(vanBanDi.Id)
                        .WithAction(EAction.UPDATE)
                        .WithStatus(vanBanDi.TrangThai?.Ten, vanBanDi.TrangThai?.Ten)
                        .WithType(ETypeHistory.Question, vanBanDi)
                        .WithTitle("Đã có thành viên từ chối")
                        .SaveChangeHistoryQuestion();
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

            await _history.WithQuestionId(vanBanDi.Id)
                .WithAction(EAction.UPDATE)
                .WithStatus(vanBanDi.TrangThai?.Ten, vanBanDi.TrangThai?.Ten)
                .WithType(ETypeHistory.Question, vanBanDi)
                .WithTitle( "Ký thành công: " + CurrentUserName)
                .SaveChangeHistoryQuestion();

            TienHanhKySo(vanBanDi.PhanCongKySo, path, vanBanDi.File.Select(x => x.FileId).ToList());
            return vanBanDi;
        }

                private bool TienHanhKySo(List<PhanCongKySo> phanCongKySos, string rootPath, List<string> fileIds)
                {
                    var userNameFormPhanCongKySo = phanCongKySos.Select(x => x.UserName).ToList();
                    var users = _context.Users.Find(x => userNameFormPhanCongKySo.Contains(x.UserName)).ToList();
                    var fileWord = _context.Files.Find(x => x.Id == fileIds.FirstOrDefault()).FirstOrDefault();
                    var kySoFunc = new KySoNoiBoService();
                    var fileName = "";
                    if (fileWord.Ext == ".docx")
                    {
                        fileName = "files" + "/" + fileWord.FileName.Replace(".docx", ".pdf");
                    }else if  (fileWord.Ext == ".doc")
                    {
                        fileName = "files"  + "/" + fileWord.FileName.Replace(".doc", ".pdf");
                    }
                 
                    
                    kySoFunc.TienTrinhKySo(fileWord.Path, fileWord.FileName, fileName ,users);
                    return false;
                }
                public async Task<List<PhanCongKySo>> GetPhanCongKySoByVanBanId(string vanBanId)
        {
            var vanBanDi = _context.VanBanDi.Find(x => x.Id == vanBanId).FirstOrDefault();
            if (vanBanDi == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            };

            return vanBanDi.PhanCongKySo.OrderByDescending(x => x.ThuTu).ToList();
        }
    }
}