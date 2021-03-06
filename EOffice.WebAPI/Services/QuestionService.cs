// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.OpenApi.Extensions;
// using MongoDB.Bson;
// using MongoDB.Driver;
// using EOffice.WebAPI.Data;
// using EOffice.WebAPI.Enums;
// using EOffice.WebAPI.Exceptions;
// using EOffice.WebAPI.Extensions;
// using EOffice.WebAPI.Helpers;
// using EOffice.WebAPI.Interfaces;
// using EOffice.WebAPI.Models;
// using EOffice.WebAPI.Params;
// using EResultResponse = EOffice.WebAPI.Exceptions.EResultResponse;
//
// namespace EOffice.WebAPI.Services
// {
//     public class QuestionService : BaseService, IQuestionService
//     {
//         private readonly DataContext _context;
//         private readonly BaseMongoDb<Question, string> BaseMongoDb;
//         private readonly IMongoCollection<Question> _collection;
//         private readonly IDbSettings _settings;
//         private ILoggingService _logger;
//         private readonly HistoryQuestionService _history;
//         private List<String> filePicture = new List<string>() {".jpeg", ".jpg" , ".gif",".png"};
//         private List<String> fileOffice = new List<string>() {".docx",".doc", ".csv" , ".xlsx",".pptx",".pdf"};
//         public QuestionService(HistoryQuestionService history, ILoggingService logger, IDbSettings settings,
//             DataContext context,
//             IHttpContextAccessor contextAccessor)
//             : base(context, contextAccessor)
//         {
//             _context = context;
//             BaseMongoDb = new BaseMongoDb<Question, string>(_context.Questions);
//             _collection = context.Questions;
//             _settings = settings;
//             _logger = logger.WithCollectionName(_settings.QuestionCollectionName)
//                 .WithDatabaseName(_settings.DatabaseName)
//                 .WithUserName(CurrentUserName);
//             _history = history;
//         }
//         public async Task<Question> Create(Question model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = new Question()
//             {
//                 Id = BsonObjectId.GenerateNewId().ToString(),
//                 Title = model.Title,
//                 Content = model.Content,
//                 LinhVuc = model.LinhVuc,
//                 Huyen = model.Huyen,
//                 Xa = model.Xa,
//                 Note = model.Note,
//                 LastedStatus = model.LastedStatus,
//                 Address = model.Address,
//                 IsPrivate = model.IsPrivate,
//                 CreatedBy = CurrentUserName,
//                 ModifiedBy = CurrentUserName,
//                 CreatedAt = DateTime.Now,
//                 ModifiedAt = DateTime.Now,
//                 UserName = CurrentUserName
//             };
//             entity.LastedStatus.StatusCode = "VTN";
//             // entity.LastedStatus.StatusName = "Ch??? duy???t";
//             entity.IdOwner = "624515a7af32dc0ade78092d";
//             if (model.UploadFiles != default && model.UploadFiles.Count > 0)
//             {
//                 foreach (var file in model.UploadFiles)
//                 {
//                     var newFile = new FileShort();
//                     newFile.FileId = file.FileId;
//                     newFile.FileName = file.FileName;
//                     newFile.Ext = file.Ext;
//                     if (fileOffice.Contains(file.Ext))
//                     {
//                         entity.FileOffice.Add(newFile);
//                     }
//                     else if (filePicture.Contains(file.Ext))
//                     {
//                         entity.FileImage.Add(newFile);
//                     }
//                     // if (entity.FileManagers == default)
//                     // {
//                     //     entity.FileManagers = new List<FileShort>();
//                     // }
//                     entity.FileManagers.Add(newFile);
//                 }
//             }
//
//             var result = await BaseMongoDb.CreateAsync(entity);
//             if (result.Entity.Id == default || !result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.CREATE_FAILURE);
//             }
//
//             await _history.WithQuestionId(entity.Id).WithUserName(entity.UserName)
//                 .WithAction(EAction.CREATE)
//                 .WithStatus(entity.LastedStatus)
//                 .WithType(ETypeHistory.Question, null)
//                 .WithTitle("Kh???i t???o ph???n ??nh/ki???n ngh???.")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//
//         public async Task<Question> Update(Question model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//
//             var entity = _context.Questions.Find(x => x.Id == model.Id).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//
//             var oldValue = entity;
//             entity.Title = model.Title;
//             entity.Content = model.Content;
//             entity.LinhVuc = model.LinhVuc;
//             entity.Huyen = model.Huyen;
//             entity.Xa = model.Xa;
//             entity.LastedStatus = model.LastedStatus;
//             entity.Address = model.Address;
//             entity.IsPrivate = model.IsPrivate;
//             entity.ModifiedBy = CurrentUserName;
//             entity.ModifiedAt = DateTime.Now;
//             entity.Note = model.Note;
//             if (model.UploadFiles != default && model.UploadFiles.Count > 0)
//             {
//                 foreach (var file in model.UploadFiles)
//                 {
//                     var newFile = new FileShort();
//                     newFile.FileId = file.FileId;
//                     newFile.FileName = file.FileName;
//                     newFile.Ext = file.Ext;
//                     if (fileOffice.Contains(file.Ext))
//                     {
//                         entity.FileOffice.Add(newFile);
//                     }
//                     else if (filePicture.Contains(file.Ext))
//                     {
//                         entity.FileImage.Add(newFile);
//                     }
//                     // if (entity.FileManagers == default)
//                     // {
//                     //     entity.FileManagers = new List<FileShort>();
//                     // }
//                     entity.FileManagers.Add(newFile);
//                 }
//             }
//             var result = await BaseMongoDb.UpdateAsync(entity);
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.UPDATE_FAILURE);
//             }
//
//             await _history.WithQuestionId(entity.Id).
//                 WithUserName(entity.UserName)
//                 .WithAction(EAction.UPDATE)
//                 .WithStatus(entity.LastedStatus)
//                 .WithType(ETypeHistory.Question, oldValue)
//                 .WithTitle("C???p nh???t ph???n ??nh/ki???n ngh???.")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//
//         public async Task Delete(string id)
//         {
//             if (id == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//
//
//             var entity = _context.Questions.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//
//             var result = await BaseMongoDb.DeleteByIdsync(id);
//
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DELETE_FAILURE);
//             }
//
//             await _history.WithQuestionId(entity.Id).
//                 WithUserName(entity.UserName)
//                 .WithType(ETypeHistory.Question, entity)
//                 .WithStatus(entity.LastedStatus)
//                 .WithAction(EAction.DELETE)
//                 .WithTitle("X??a ph???n ??nh/ki???n ngh???.")
//                 .SaveChangeHistoryQuestion();
//         }
//
//         public async Task<List<Question>> Get()
//         {
//             return await _context.Questions.Find(x => x.IsDeleted != true)
//                 .ToListAsync();
//         }
//
//         public async Task<Question> GetById(string id)
//         {
//             var model =  await _context.Questions.Find(x => x.Id == id && x.IsDeleted != true)
//                 .FirstOrDefaultAsync();
//                 if (IdDonVi == model.IdOwner && model.IdOwner != null )
//             {
//                 model.isShow = true;
//             }
//             return model;
//         }
//
//         public async Task<PagingModel<Question>> GetPaging(QuestionParamFilter param)
//         {
//             PagingModel<Question> result = new PagingModel<Question>();
//             var builder = Builders<Question>.Filter;
//             var filter = builder.Empty;
//             filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
//             filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
//             if (!String.IsNullOrEmpty(param.Title))
//             {
//                 filter = builder.And(filter,
//                     builder.Where(x => x.Title.Trim().ToLower().Contains(param.Title.Trim().ToLower())));
//             }
//             if (!String.IsNullOrEmpty(param.StatusId))
//             {
//                 filter = builder.And(filter, builder.Eq(x => x.LastedStatus.StatusCode, param.StatusId.ToUpper()));
//             }
//             if (!String.IsNullOrEmpty(param.LinhVucId))
//             {
//                 filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVucId));
//             }
//             if (param.StartDate != null)
//             {
//                 DateTime startDate = DateTime.SpecifyKind((DateTime) param.StartDate, DateTimeKind.Utc);
//                 DateTime endDate = DateTime.SpecifyKind((DateTime) param.EndDate?.AddDays(1), DateTimeKind.Utc);
//                 filter = filter & builder.Gte(x => x.CreatedAt, startDate)
//                                 & builder.Lt(x => x.CreatedAt, endDate);
//             }
//             string sortBy = nameof(Question.CreatedAt);
//             result.TotalRows = await _collection.CountDocumentsAsync(filter);
//             result.Data = await _collection.Find(filter)
//                 .Sort(param.SortDesc
//                     ? Builders<Question>
//                         .Sort.Descending(sortBy)
//                     : Builders<Question>
//                         .Sort.Ascending(sortBy))
//                 .Skip(param.Skip)
//                 .Limit(param.Limit)
//                 .ToListAsync(); 
//             return result;
//         }
//         public async Task<PagingModel<Question>> GetPagingReceive(QuestionParamFilter param)
//         {
//             PagingModel<Question> result = new PagingModel<Question>();
//             var builder = Builders<Question>.Filter;
//             var filter = builder.Empty;
//             filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
//             filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
//             filter = builder.And(filter, builder.Where(x => x.LastedStatus.StatusCode == "VTN"));
//             if (!String.IsNullOrEmpty(param.Title))
//             {
//                 filter = builder.And(filter,
//                     builder.Where(x => x.Title.Trim().ToLower().Contains(param.Title.Trim().ToLower())));
//             }
//            
//             if (!String.IsNullOrEmpty(param.LinhVucId))
//             {
//                 filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVucId));
//             }
//             if (param.StartDate != null)
//             {
//                 DateTime startDate = DateTime.SpecifyKind((DateTime) param.StartDate, DateTimeKind.Utc);
//                 DateTime endDate = DateTime.SpecifyKind((DateTime) param.EndDate?.AddDays(1), DateTimeKind.Utc);
//                 filter = filter & builder.Gte(x => x.CreatedAt, startDate)
//                                 & builder.Lt(x => x.CreatedAt, endDate);
//             }
//             string sortBy = nameof(Question.CreatedAt);
//             result.TotalRows = await _collection.CountDocumentsAsync(filter);
//             result.Data = await _collection.Find(filter)
//                 .Sort(param.SortDesc
//                     ? Builders<Question>
//                         .Sort.Descending(sortBy)
//                     : Builders<Question>
//                         .Sort.Ascending(sortBy))
//                 .Skip(param.Skip)
//                 .Limit(param.Limit)
//                 .ToListAsync();
//             return result;
//         }
//         
//         
//         public async Task<PagingModel<Question>> GetPagingHandle(QuestionParamFilter param)
//         {
//             PagingModel<Question> result = new PagingModel<Question>();
//             var builder = Builders<Question>.Filter;
//             var filter = builder.Empty;
//             filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
//             filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);
//             filter = builder.And(filter, builder.Where(x => x.LastedStatus.StatusCode == "CYC"));
//             if (!String.IsNullOrEmpty(param.Title))
//             {
//                 filter = builder.And(filter,
//                     builder.Where(x => x.Title.Trim().ToLower().Contains(param.Title.Trim().ToLower())));
//             }
//             if (!String.IsNullOrEmpty(param.LinhVucId))
//             {
//                 filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVucId));
//             }
//             if (param.StartDate != null)
//             {
//                 DateTime startDate = DateTime.SpecifyKind((DateTime) param.StartDate, DateTimeKind.Utc);
//                 DateTime endDate = DateTime.SpecifyKind((DateTime) param.EndDate?.AddDays(1), DateTimeKind.Utc);
//                 filter = filter & builder.Gte(x => x.CreatedAt, startDate)
//                                 & builder.Lt(x => x.CreatedAt, endDate);
//             }
//             string sortBy = nameof(Question.CreatedAt);
//             result.TotalRows = await _collection.CountDocumentsAsync(filter);
//             result.Data = await _collection.Find(filter)
//                 .Sort(param.SortDesc
//                     ? Builders<Question>
//                         .Sort.Descending(sortBy)
//                     : Builders<Question>
//                         .Sort.Ascending(sortBy))
//                 .Skip(param.Skip)
//                 .Limit(param.Limit)
//                 .ToListAsync();
//             return result;
//         }
//
//         public async Task ChangeStatusQuestion(StatusQuestion model)
//         {
//             if (model == default)
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//
//             var question = _context.Questions.Find(x => x.Id == model.QuestionId).FirstOrDefault();
//             if (question == default)
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             question.LastedStatus = model;
//             var result = await BaseMongoDb.UpdateAsync(question);
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage("Chuy???n tr???ng th??i th???t b???i!");
//             }
//
//             await _history.WithQuestionId(question.Id)
//                 .WithType(ETypeHistory.Question, question)
//                 .WithStatus(question.LastedStatus)
//                 .WithAction(EAction.CHANGESTATUS)
//                 .WithTitle("Chuy???n tr???ng th??i ph???n ??nh")
//                 .SaveChangeHistoryQuestion();
//         }
//
//         public async Task<Question> QuestionNavigation(Question model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = _context.Questions.Find(x => x.Id == model.Id).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//             var oldValue = entity;
//             entity.DonVi = model.DonVi;
//             entity.LastedStatus.StatusCode = model.LastedStatus.StatusCode;
//             entity.IdOwner = model.DonVi.Id;
//             entity.ModifiedBy = CurrentUserName;
//             entity.ModifiedAt = DateTime.Now;
//             var result = await BaseMongoDb.UpdateAsync(entity);
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.UPDATE_FAILURE);
//             }
//             await _history.WithQuestionId(entity.Id).
//                 WithUserName(entity.UserName)
//                 .WithAction(EAction.CHANGESTATUS)
//                 .WithStatus(entity.LastedStatus)
//                 .WithType(ETypeHistory.Question, oldValue)
//                 .WithTitle("Chuy???n tr???ng th??i v???a ti???p nh???n")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//
//         public async Task<Question> NotApprove(Question model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = _context.Questions.Find(x => x.Id == model.Id).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//             var oldValue = entity;
//             entity.Note = model.Note;
//             entity.LastedStatus.StatusCode = DefineStatus.KHONGTIEPNHAN.ToString();
//             entity.LastedStatus.StatusName = "Kh??ng ti???p nh???n";
//             entity.ModifiedBy = CurrentUserName;
//             entity.ModifiedAt = DateTime.Now;
//             var result = await BaseMongoDb.UpdateAsync(entity);
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.UPDATE_FAILURE);
//             }
//
//             await _history.WithQuestionId(entity.Id).
//                 WithUserName(entity.UserName)
//                 .WithAction(EAction.CHANGESTATUS)
//                 .WithStatus(entity.LastedStatus)
//                 .WithType(ETypeHistory.Question, oldValue)
//                 .WithTitle("Chuy???n tr???ng th??i kh??ng ti???p nh???n.")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//     }
// }
