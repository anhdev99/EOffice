using System;
using System.Collections.Generic;
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
    public class VanBanDiService : BaseService, IQuestionService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<VanBanDi, string> BaseMongoDb;
        private readonly IMongoCollection<VanBanDi> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;
        private readonly HistoryVanBanDiService _history;
        private List<String> filePicture = new List<string>() {".jpeg", ".jpg" , ".gif",".png"};
        private List<String> fileOffice = new List<string>() {".docx",".doc", ".csv" , ".xlsx",".pptx",".pdf"};
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
            
            
            if (model.UploadFiles != default )
            {
                var newFile = new FileShort();
                newFile.FileId = model.UploadFiles.FileId;
                newFile.FileName = model.UploadFiles.FileName;
                newFile.Ext = model.UploadFiles.Ext;
                entity.File = newFile;
            }

            var donVi = _context.DonVis.Find(x => x.IsDeleted != true).ToList();
            entity.DonViSoanTen = donVi.Find(x => x.Id == model.DonViSoan)?.Ten;
            entity.DonViSoanTen = model.DonViSoan;
            
            entity.CoQuanNhanTen = donVi.Find(x => x.Id == model.CoQuanNhan)?.Ten;
            entity.CoQuanNhan = model.CoQuanNhan;
            
            entity.KhoiCoQuanNhanTen = donVi.Find(x => x.Id == model.KhoiCoQuanNhan)?.Ten;
            entity.KhoiCoQuanNhan = model.KhoiCoQuanNhan;

            var canBo = _context.Users.Find(x => x.Id == model.CanBoSoan).FirstOrDefault();
            entity.CanBoSoan = model.CanBoSoan;
            entity.CanBoSoanTen = canBo?.UserName  +"-" + canBo?.FullName;

            entity.HinhThucGui = 
            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            await _history.WithQuestionId(entity.Id).WithUserName(entity.UserName)
                .WithAction(EAction.CREATE)
                .WithStatus(entity.LastedStatus)
                .WithType(ETypeHistory.Question, null)
                .WithTitle("Khởi tạo phản ánh/kiến nghị.")
                .SaveChangeHistoryQuestion();
            return entity;
        }

        public async Task<Question> Update(Question model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.Questions.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var oldValue = entity;
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.LinhVuc = model.LinhVuc;
            entity.Huyen = model.Huyen;
            entity.Xa = model.Xa;
            entity.LastedStatus = model.LastedStatus;
            entity.Address = model.Address;
            entity.IsPrivate = model.IsPrivate;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            entity.Note = model.Note;
            if (model.UploadFiles != default && model.UploadFiles.Count > 0)
            {
                foreach (var file in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = file.FileId;
                    newFile.FileName = file.FileName;
                    newFile.Ext = file.Ext;
                    if (fileOffice.Contains(file.Ext))
                    {
                        entity.FileOffice.Add(newFile);
                    }
                    else if (filePicture.Contains(file.Ext))
                    {
                        entity.FileImage.Add(newFile);
                    }
                    // if (entity.FileManagers == default)
                    // {
                    //     entity.FileManagers = new List<FileShort>();
                    // }
                    entity.FileManagers.Add(newFile);
                }
            }
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithQuestionId(entity.Id).
                WithUserName(entity.UserName)
                .WithAction(EAction.UPDATE)
                .WithStatus(entity.LastedStatus)
                .WithType(ETypeHistory.Question, oldValue)
                .WithTitle("Cập nhật phản ánh/kiến nghị.")
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


            var entity = _context.Questions.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

            await _history.WithQuestionId(entity.Id).
                WithUserName(entity.UserName)
                .WithType(ETypeHistory.Question, entity)
                .WithStatus(entity.LastedStatus)
                .WithAction(EAction.DELETE)
                .WithTitle("Xóa phản ánh/kiến nghị.")
                .SaveChangeHistoryQuestion();
        }

        public async Task<List<Question>> Get()
        {
            return await _context.Questions.Find(x => x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<Question> GetById(string id)
        {
            var model =  await _context.Questions.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
                if (IdDonVi == model.IdOwner && model.IdOwner != null )
            {
                model.isShow = true;
            }
            return model;
        }

        public async Task<PagingModel<Question>> GetPaging(QuestionParamFilter param)
        {
            PagingModel<Question> result = new PagingModel<Question>();
            var builder = Builders<Question>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            if (!String.IsNullOrEmpty(param.Title))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Title.Trim().ToLower().Contains(param.Title.Trim().ToLower())));
            }
            if (!String.IsNullOrEmpty(param.StatusId))
            {
                filter = builder.And(filter, builder.Eq(x => x.LastedStatus.StatusCode, param.StatusId.ToUpper()));
            }
            if (!String.IsNullOrEmpty(param.LinhVucId))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVucId));
            }
            if (param.StartDate != null)
            {
                DateTime startDate = DateTime.SpecifyKind((DateTime) param.StartDate, DateTimeKind.Utc);
                DateTime endDate = DateTime.SpecifyKind((DateTime) param.EndDate?.AddDays(1), DateTimeKind.Utc);
                filter = filter & builder.Gte(x => x.CreatedAt, startDate)
                                & builder.Lt(x => x.CreatedAt, endDate);
            }
            string sortBy = nameof(Question.CreatedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<Question>
                        .Sort.Descending(sortBy)
                    : Builders<Question>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync(); 
            return result;
        }
        public async Task<PagingModel<Question>> GetPagingReceive(QuestionParamFilter param)
        {
            PagingModel<Question> result = new PagingModel<Question>();
            var builder = Builders<Question>.Filter;
            var filter = builder.Empty;
            filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            filter = builder.And(filter, builder.Where(x => x.LastedStatus.StatusCode == "VTN"));
            if (!String.IsNullOrEmpty(param.Title))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Title.Trim().ToLower().Contains(param.Title.Trim().ToLower())));
            }
           
            if (!String.IsNullOrEmpty(param.LinhVucId))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVucId));
            }
            if (param.StartDate != null)
            {
                DateTime startDate = DateTime.SpecifyKind((DateTime) param.StartDate, DateTimeKind.Utc);
                DateTime endDate = DateTime.SpecifyKind((DateTime) param.EndDate?.AddDays(1), DateTimeKind.Utc);
                filter = filter & builder.Gte(x => x.CreatedAt, startDate)
                                & builder.Lt(x => x.CreatedAt, endDate);
            }
            string sortBy = nameof(Question.CreatedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<Question>
                        .Sort.Descending(sortBy)
                    : Builders<Question>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
        
        
        public async Task<PagingModel<Question>> GetPagingHandle(QuestionParamFilter param)
        {
            PagingModel<Question> result = new PagingModel<Question>();
            var builder = Builders<Question>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
            filter = builder.And(filter, builder.Where(x => x.LastedStatus.StatusCode == "CYC"));
            if (!String.IsNullOrEmpty(param.Title))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Title.Trim().ToLower().Contains(param.Title.Trim().ToLower())));
            }
            if (!String.IsNullOrEmpty(param.LinhVucId))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVucId));
            }
            if (param.StartDate != null)
            {
                DateTime startDate = DateTime.SpecifyKind((DateTime) param.StartDate, DateTimeKind.Utc);
                DateTime endDate = DateTime.SpecifyKind((DateTime) param.EndDate?.AddDays(1), DateTimeKind.Utc);
                filter = filter & builder.Gte(x => x.CreatedAt, startDate)
                                & builder.Lt(x => x.CreatedAt, endDate);
            }
            string sortBy = nameof(Question.CreatedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<Question>
                        .Sort.Descending(sortBy)
                    : Builders<Question>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task ChangeStatusQuestion(StatusQuestion model)
        {
            if (model == default)
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);

            var question = _context.Questions.Find(x => x.Id == model.QuestionId).FirstOrDefault();
            if (question == default)
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            question.LastedStatus = model;
            var result = await BaseMongoDb.UpdateAsync(question);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Chuyển trạng thái thất bại!");
            }

            await _history.WithQuestionId(question.Id)
                .WithType(ETypeHistory.Question, question)
                .WithStatus(question.LastedStatus)
                .WithAction(EAction.CHANGESTATUS)
                .WithTitle("Chuyển trạng thái phản ánh")
                .SaveChangeHistoryQuestion();
        }

        public async Task<Question> QuestionNavigation(Question model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var entity = _context.Questions.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }
            var oldValue = entity;
            entity.DonVi = model.DonVi;
            entity.LastedStatus.StatusCode = model.LastedStatus.StatusCode;
            entity.IdOwner = model.DonVi.Id;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }
            await _history.WithQuestionId(entity.Id).
                WithUserName(entity.UserName)
                .WithAction(EAction.CHANGESTATUS)
                .WithStatus(entity.LastedStatus)
                .WithType(ETypeHistory.Question, oldValue)
                .WithTitle("Chuyển trạng thái vừa tiếp nhận")
                .SaveChangeHistoryQuestion();
            return entity;
        }

        public async Task<Question> NotApprove(Question model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var entity = _context.Questions.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }
            var oldValue = entity;
            entity.Note = model.Note;
            entity.LastedStatus.StatusCode = DefineStatus.KHONGTIEPNHAN.ToString();
            entity.LastedStatus.StatusName = "Không tiếp nhận";
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _history.WithQuestionId(entity.Id).
                WithUserName(entity.UserName)
                .WithAction(EAction.CHANGESTATUS)
                .WithStatus(entity.LastedStatus)
                .WithType(ETypeHistory.Question, oldValue)
                .WithTitle("Chuyển trạng thái không tiếp nhận.")
                .SaveChangeHistoryQuestion();
            return entity;
        }
    }
}
