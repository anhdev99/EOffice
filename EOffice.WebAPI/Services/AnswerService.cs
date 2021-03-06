// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
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
//     public class AnswerService : BaseService, IAnswerService
//     {
//         private DataContext _context;
//         private BaseMongoDb<Answer, string> BaseMongoDb;
//         private IMongoCollection<Answer> _collection;
//         private IDbSettings _settings;
//         private ILoggingService _logger;
//         private IQuestionService _question;
//         private HistoryQuestionService _history;
//         private List<String> filePicture = new List<string>() {".jpeg", ".jpg" , ".gif",".png"};
//         private List<String> fileOffice = new List<string>() {".docx",".doc", ".csv" , ".xlsx",".pptx",".pdf"};
//         public AnswerService(HistoryQuestionService history, ILoggingService logger, IDbSettings settings, DataContext context, 
//             IQuestionService questionService, 
//             IHttpContextAccessor contextAccessor)
//             : base(context, contextAccessor)
//         {
//             _context = context;
//             BaseMongoDb = new BaseMongoDb<Answer, string>(_context.Answers);
//             _collection = context.Answers;
//             _settings = settings;
//             _question = questionService;
//             _logger = logger.WithCollectionName(_settings.QuestionCollectionName)
//                 .WithDatabaseName(_settings.DatabaseName)
//                 .WithUserName(CurrentUserName);
//             _history = history;
//         }
//
//         public async Task<Answer> Create(Answer model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = new Answer()
//             {
//                 Content = model.Content,
//                 ListLink = model.ListLink,
//                 UserName = CurrentUserName,
//                 FullName = CurrentUser.FullName,
//                 DepartmentId = CurrentUser.DonVi?.Id,
//                 DepartmentName = CurrentUser.DonVi?.Ten,
//                 QuestionId = model.QuestionId,
//                 ModifiedAt = DateTime.Now ,
//                 CreatedAt = DateTime.Now,
//                 CreatedBy = CurrentUserName,
//                 ModifiedBy = CurrentUserName
//             };
//             if (model.UploadFiles!= default && model.UploadFiles.Count > 0)
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
//                     entity.FileManagers.Add(newFile);
//                 }
//             }
//             var result = await BaseMongoDb.CreateAsync(entity);
//             if (result.Entity.Id == default || !result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.CREATE_FAILURE);
//             }
//             var status = new StatusQuestion();
//             status.StatusCode = model.Status.StatusCode;
//             status.QuestionId = entity.QuestionId;
//             await  _question.ChangeStatusQuestion(status);
//             var question = _context.Questions.Find(x => x.Id == entity.QuestionId).FirstOrDefault();
//             await _history.WithQuestionId(entity.QuestionId).WithUserName(entity.UserName)
//                 .WithStatus(question.LastedStatus)
//                 .WithAction(EAction.CREATE)
//                 .WithType(ETypeHistory.Answer, null)
//                 .WithTitle("Tr??? l???i ph???n ??nh/ki???n ngh???.")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//         public async Task<Answer> Update(Answer  model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = _context.Answers.Find(x => x.Id == model.Id).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//             var oldValue = entity;
//             entity.Content = model.Content;
//             entity.ListLink = model.ListLink;
//             entity.UserName = CurrentUserName;
//             entity.FullName = CurrentUser.FullName;
//             entity.DepartmentId = CurrentUser.DonVi?.Id;
//             entity.DepartmentName = CurrentUser.DonVi?.Ten;
//             entity.ModifiedBy = CurrentUserName;
//             if (model.UploadFiles != default && model.UploadFiles.Count > 0)
//             {
//                 foreach (var file in model.UploadFiles)
//                 {
//                     var newFile = new FileShort();
//                     newFile.FileId = file.FileId;
//                     newFile.FileName = file.FileName;
//                     if (fileOffice.Contains(file.Ext))
//                     {
//                         entity.FileOffice.Add(newFile);
//                     }
//                     else if (filePicture.Contains(file.Ext))
//                     {
//                         entity.FileImage.Add(newFile);
//                     }
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
//             var question = _context.Questions.Find(x => x.Id == entity.QuestionId).FirstOrDefault();
//             
//             await _history.WithQuestionId(entity.QuestionId)
//                 .WithUserName(entity.UserName)
//                 .WithStatus(question.LastedStatus)
//                 .WithAction(EAction.UPDATE)
//                 .WithType(ETypeHistory.Answer, oldValue)
//                 .WithTitle("C???p nh???t tr??? l???i ph???n ??nh/ki???n ngh???.")
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
//             var entity = _context.Answers.Find(x => x.Id == id).FirstOrDefault();
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
//             var question = _context.Questions.Find(x => x.Id == entity.QuestionId).FirstOrDefault();
//             await _history.WithQuestionId(entity.QuestionId).WithUserName(entity.UserName)
//                 .WithAction(EAction.DELETE)
//                 .WithStatus(question?.LastedStatus)
//                 .WithType(ETypeHistory.Answer, entity)
//                 .WithTitle("X??a tr??? l???i ph???n ??nh/ki???n ngh???.")
//                 .SaveChangeHistoryQuestion();
//         }
//
//         public async Task<List<Answer>> Get(string questionId)
//         {
//             return await _context.Answers.Find(x => x.QuestionId == questionId)
//                 .ToListAsync();
//         }
//
//         public async Task<Answer> GetByIdQuestion(string id)
//         {
//             return await _context.Answers.Find(x => x.QuestionId == id)
//                 .FirstOrDefaultAsync();
//         }
//
//         public async Task<PagingModel<Answer>> GetPaging(AnswerParam param)
//         {
//             PagingModel<Answer> result = new PagingModel<Answer>();
//             var builder = Builders<Answer>.Filter;
//             var filter = builder.Empty;
//
//             filter = builder.And(filter, builder.Where(x => x.QuestionId == param.QuestionId));
//             if (!String.IsNullOrEmpty(param.Content))
//             {
//                 filter = builder.And(filter,
//                     builder.Where(x => x.Content.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
//             }
//
//             string sortBy = nameof(Answer.CreatedAt);
//             result.TotalRows = await _collection.CountDocumentsAsync(filter);
//             result.Data = await _collection.Find(filter)
//                 .Sort(param.SortDesc
//                     ? Builders<Answer>
//                         .Sort.Descending(sortBy)
//                     : Builders<Answer>
//                         .Sort.Ascending(sortBy))
//                 .Skip(param.Skip)
//                 .Limit(param.Limit)
//                 .ToListAsync();
//             return result;
//         }
//     }
// }
