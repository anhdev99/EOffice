using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
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
    public class NotifyService : BaseService, INotifyService
    {
        private DataContext _context;
        private IMongoCollection<Notify> _collection;
        private BaseMongoDb<Notify, string> BaseMongoDb;
        public NotifyService(DataContext context, IHttpContextAccessor contextAccessor)
            : base(context,contextAccessor)
        {
            _context = context;
            _collection = _context.Notify;
            BaseMongoDb = new BaseMongoDb<Notify, string>(context.Notify);
        }
        
        private List<string> _recipientIds { get; set; }
        
        private Notify _notify;

        public NotifyService WithNotify(Notify notify)
        {
            if (notify != default)
            {
                _notify = notify;
            }
            return this;
        }

        public NotifyService WithRecipients(List<string> recipientIds)
        {
            if (recipientIds != default)
            {
                _recipientIds = recipientIds;
            }

            return this;
        }

        public NotifyService WithRecipient(string recipientId)
        {
            if (!string.IsNullOrEmpty(recipientId))
            {
                _recipientIds = new List<string>()
                {
                    recipientId
                };
            }

            return this;
        }

        public async Task PushNotify()
        {
            if (_recipientIds == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Recipient not empty");
            }

            if (_notify == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Notifycation not empty");
            }

            var users = _context.Users.Find(x => _recipientIds.Contains(x.Id) && x.IsDeleted != true).ToList();
            foreach (var user in users)
            {
                _notify.Id = BsonObjectId.GenerateNewId().ToString();
                _notify.Sender = CurrentUser.FullName;
                _notify.SenderId = CurrentUser.Id;
                _notify.Recipient = user.FullName;
                _notify.RecipientId = user.Id;
                
                var result = await BaseMongoDb.CreateAsync(_notify);
            }

            _notify = default;
            _recipientIds = default;
        }
        public async Task<PagingModel<Notify>> GetPaging(NotifyParam param)
        {
            // var result = new PagingModel1<User>();
            // var query = _context.Users.AsQueryable().Where(x => x.IsDeleted != true).OrderByDescending(x => x.ModifiedAt);
            // var data = PagedList<User>.ToPagedList(query, param.Start, param.Limit);
            // return new PagingModel1<User>(data);
            PagingModel<Notify> result = new PagingModel<Notify>();
            var builder = Builders<Notify>.Filter;
            var filter = builder.Empty;
            try
            {
                filter = builder.And(filter, builder.Where(x => x.RecipientId == CurrentUser.Id));
                if (!String.IsNullOrEmpty(param.Content))
                {
                    filter = builder.And(filter,
                        builder.Where(x => x.Title.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
                }
                string sortBy = nameof(User.ModifiedAt);
                result.TotalRows = await _context.Notify.CountDocumentsAsync(filter);
                result.Data = await _context.Notify.Find(filter)
                    .Sort(param.SortDesc
                        ? Builders<Notify>
                            .Sort.Descending(sortBy)
                        : Builders<Notify>
                            .Sort.Ascending(sortBy))
                    .Skip((param.Start > 0 ? param.Start - 1 : 0) * param.Limit)
                    .Limit(param.Limit)
                    .ToListAsync();
                return result;
            } catch (Exception ex)
            {
                result.TotalRows = 0;
                result.Data = null;
            }
            return result;
        }
        
        public async Task<Notify> GetById(string id)
        {
            return await _context.Notify.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ResultResponse<NotifyVM>> GetListNotify()
        {
            ResultResponse<NotifyVM> resultResponse = new ResultResponse<NotifyVM>();

            var builder = Builders<Notify>.Filter;
            var filter = builder.Empty;
            try
            {
                var notifyVm = new NotifyVM();      
		        filter = builder.And(filter, builder.Where(x => x.RecipientId == CurrentUser.Id));
		        filter = builder.And(filter, builder.Where(x => x.Read == false));
                string sortBy = nameof(User.ModifiedAt);
                var TotalRows = await _context.Notify.CountDocumentsAsync(filter);
                var data = await _context.Notify.Find(filter)
                    .Skip(0)
                    .Limit(5)
                    .ToListAsync();
                notifyVm.Number = TotalRows;
                notifyVm.ListNotify = data;

                resultResponse.ResultCode = EResultResponse.SUCCESS.ToString();
                resultResponse.ResultString = "Lấy thông báo thành công";
                resultResponse.Data = notifyVm;
                
	        }
	        catch (Exception ex)
	        {

		        resultResponse.ResultCode = EResultResponse.ERROR.ToString();
                resultResponse.ResultString = "Lỗi" + ex.Message;
            }

            return resultResponse;
        }

        public async Task<ResultResponse<Notify>> ChangeStatus(Notify model)
        {
            ResultResponse<Notify> resultResponse = new ResultResponse<Notify>();
            if (model.Id == null || model.Id == "")
            {
                resultResponse.ResultCode = EResultResponse.FAIL.ToString();
                resultResponse.ResultString = "Dữ liệu không được để trống.";
                return resultResponse;
            }
            try
            {
                var entity = await _collection.Find(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (entity == default)
                {
                    resultResponse.ResultCode = EResultResponse.NOT_EXIST.ToString();
                    resultResponse.ResultString = "Không tồn tại dữ liệu.";
                    return resultResponse;
                }

                entity.Read = true;
                var result = await BaseMongoDb.UpdateAsync(entity);
                if (result.Success)
                {
                    resultResponse.ResultCode = EResultResponse.SUCCESS.ToString();
                    resultResponse.ResultString = "Cập nhật thành công.";
                    resultResponse.Data = entity;
                    return resultResponse;
                }

                resultResponse.ResultCode = EResultResponse.FAIL.ToString();
                resultResponse.ResultString = "Cập nhật không thành công.";

            }
            catch (Exception ex)
            {
                resultResponse.ResultCode = EResultResponse.SUCCESS.ToString();
                resultResponse.ResultString = "Lỗi" + ex.Message;
            }
            return resultResponse;
        }
    }
}