using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class LichCongTacService : BaseService, ILichCongTacService
    {
        private DataContext _context;
        private BaseMongoDb<LichCongTac, string> BaseMongoDb;
        private IMongoCollection<LichCongTac> _collection;
        private IDbSettings _settings;
        private INotifyService _notifyService;
        private ILoggingService _logger;

        public LichCongTacService(ILoggingService logger, IDbSettings settings, DataContext context,INotifyService notifyService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LichCongTac, string>(_context.LichCongTac);
            _collection = context.LichCongTac;
            _settings = settings;
            _notifyService = notifyService;
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
                LoaiLichCongTac = model.LoaiLichCongTac,
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

            try
            {
                
                var notify = new Notify()
                {
                    Title =
                        $"B???n c?? m???t l???ch c??ng t??c m???i!",
                    Content =
                        $"N???i dung: B???n c?? m???t l???ch c??ng t??c v??o ng??y {entity.NgayXepLich.ToShortDateString()} <a href='/xem-lich-cong-tac'> Xem l???ch c??ng t??c</a>"
                };

                var listUser = entity.ChuTri.Select(x => x.Id).ToList();
                await  _notifyService.WithNotify(notify).WithRecipients(listUser).PushNotify();
            }
            catch (Exception e)
            {
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
            entity.LoaiLichCongTac = model.LoaiLichCongTac;
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

            try
            {
                
                var notify = new Notify()
                {
                    Title =
                        $"B???n c?? m???t l???ch c??ng t??c m???i!",
                    Content =
                        $"N???i dung: B???n c?? m???t l???ch c??ng t??c v??o ng??y {entity.NgayXepLich.ToShortDateString()} <a href='/xem-lich-cong-tac'> Xem l???ch c??ng t??c</a>"
                };

                var listUser = entity.ChuTri.Select(x => x.Id).ToList();
                await  _notifyService.WithNotify(notify).WithRecipients(listUser).PushNotify();
            }
            catch (Exception e)
            {
            }
            return entity;
        }

        public async Task<PagingModel<LichCongTac>> GetPagingCaNhan(LichCongTacParam param)
        {
            var result = new PagingModel<LichCongTac>();
            var builder = Builders<LichCongTac>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(param.LoaiLichCongTac))
            {
                filter = builder.And(filter, builder.Where(x =>x.CreatedBy == CurrentUserName && x.LoaiLichCongTac == param.LoaiLichCongTac && x.IsDeleted == false));
            }
            else
            {
                filter = builder.And(filter, builder.Where(x =>x.CreatedBy == CurrentUserName && x.LoaiLichCongTac == null && x.IsDeleted == false));
            }

            string sortBy = nameof(LichCongTac.NgayXepLich);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LichCongTac>
                        .Sort.Descending(sortBy)
                    : Builders<LichCongTac>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
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
        public async Task<dynamic> GetAll()
        {
            CultureInfo vietNam = new CultureInfo("vi-VN");
            var data = await _context.LichCongTac.Find(x => x.IsDeleted != true)
                .ToListAsync();
            var results = data.GroupBy(
                p => p.NgayXepLich, 
                p => p,
                (key, g) => new { NgayXepLich = key.ToString("dddd", vietNam) + " ng??y " + key.ToString("dd/MM/yyyy", vietNam) + $"{GetDateOfChineseNewYear(key.ToLocalTime())}", LichCongTac = g.ToList() });

            var lichCongTac = new List<LichCongTacVM>();
            foreach (var item in results)
            {
                var itemLCT = new LichCongTacVM();
                itemLCT.NgayXepLich = item.NgayXepLich;
            
                foreach (var lct in item.LichCongTac)
                {
                    if (itemLCT.CongViecs == default)
                    {
                        itemLCT.CongViecs = new List<CongViec>();
                    }

                    int rowspan = 1;
                    bool first = true;
                    foreach (CongViec cv in lct.CongViecs)
                    {
                        if (first)
                        {
                            first = false;
                            cv.ChuTri = lct.ChuTri;
                            cv.RowSpan = lct.CongViecs.Count;
                        }

                      
                        itemLCT.CongViecs.Add(cv);
                    }
 
                }
                lichCongTac.Add(itemLCT);
                
            }
            return lichCongTac;
        }
        
        public async Task<dynamic> GetPaging(LichCongTacParam param)
        {
            var builder = Builders<LichCongTac>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (param.SelectDay != default)
            {
                DateTime startDate = DateTime.SpecifyKind((DateTime)param.SelectDay, DateTimeKind.Utc);
                DateTime endDate = startDate.AddDays(6);
                filter = builder.And(filter, builder.Where(x => x.NgayXepLich <= endDate && x.NgayXepLich >= startDate));
            }
            else
            {
                DateTime startDate = DateTime.Now;
                DateTime endDate = startDate.AddDays(6);
                filter = builder.And(filter, builder.Where(x => x.NgayXepLich <= endDate && x.NgayXepLich >= startDate));
            }

            if (!String.IsNullOrEmpty(param.LoaiLichCongTac))
            {
                if (param.LoaiLichCongTac == "lctt")
                {
                    filter = builder.And(filter,
                        builder.Where(x => x.LoaiLichCongTac == param.LoaiLichCongTac));
                }
                else if (param.LoaiLichCongTac == "lctdv")
                {
                    var currentDonVi = CurrentUser.DonVi;
                    filter = builder.And(filter,
                        builder.Where(x => x.ChuTri.Any(x => x.DonVi != default && x.DonVi.Id == currentDonVi.Id) && x.LoaiLichCongTac == param.LoaiLichCongTac));
                }
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x => x.ChuTri.Any(x => x.UserName == CurrentUserName) || (x.CongViecs != default && x.CongViecs.Any(p => p.ThanhPhanThamDu.Any(e => e.UserName == CurrentUserName)))));
            }
            CultureInfo vietNam = new CultureInfo("vi-VN");
            var data = await _context.LichCongTac.Find(filter).SortBy(x => x.NgayXepLich)
                .ToListAsync();
            var results = data.GroupBy(
                p => p.NgayXepLich, 
                p => p,
                (key, g) => new { NgayXepLich = key.ToString("dddd", vietNam) + " ng??y " + key.ToString("dd/MM/yyyy", vietNam) + $"{GetDateOfChineseNewYear(key.ToLocalTime())}", LichCongTac = g.ToList() });

            var lichCongTac = new List<LichCongTacVM>();
            foreach (var item in results)
            {
                var itemLCT = new LichCongTacVM();
                itemLCT.NgayXepLich = item.NgayXepLich;
            
                foreach (var lct in item.LichCongTac)
                {
                    if (itemLCT.CongViecs == default)
                    {
                        itemLCT.CongViecs = new List<CongViec>();
                    }

                    int rowspan = 1;
                    bool first = true;
                    if (lct.CongViecs != default)
                    {
                        foreach (CongViec cv in lct.CongViecs)
                        {
                            if (first)
                            {
                                first = false;
                                cv.ChuTri = lct.ChuTri;
                                cv.RowSpan = lct.CongViecs.Count;
                            }

                      
                            itemLCT.CongViecs.Add(cv);
                        }
                    }
       
 
                }
                lichCongTac.Add(itemLCT);
                
            }
            return lichCongTac;
        }
        #region CongViec
        public string GetDateOfChineseNewYear(DateTime date)
        {
            ChineseLunisolarCalendar chinese   = new ChineseLunisolarCalendar();

            DateTime utcNow = date;
            Int32 year  = chinese.GetYear( date );
            Int32 month = chinese.GetMonth( date );
            Int32 day   = chinese.GetDayOfMonth( date );

            var dayString = day.ToString();
            var monthString = month.ToString();
            if (day < 10)
            {
                dayString = "0" + day.ToString();
            }
            if (month < 10)
            {
                monthString = "0" + month.ToString();
            }
            return $" ({dayString}/{monthString}/{year.ToString()}  ??m l???ch)";
        }
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
                    .WithMessage("Kh??ng t??m th???y l???ch c??ng t??c.");
            }

            if (lichCongTac.CongViecs == default)
            {
                lichCongTac.CongViecs = new List<CongViec>();
            }

            model.Id = BsonObjectId.GenerateNewId().ToString();
            lichCongTac.CongViecs.Add(model);

            var result = await BaseMongoDb.UpdateAsync(lichCongTac);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Th??m c??ng vi???c kh??ng th??nh c??ng!");
            }

            // Th??ng b??o ?????n ng?????i tham d???
            try
            {
                var listUser = new List<string>();
                foreach (var item in  lichCongTac.CongViecs)
                {
                    var notify = new Notify()
                    {
                        Title =
                            $"B???n c?? m???t l???ch c??ng t??c m???i!",
                        Content =
                            $"B???n c?? m???t l???ch c??ng t??c v??o ng??y {item.TuNgay?.ToShortDateString()} <a href='/xem-lich-cong-tac'> Xem l???ch c??ng t??c</a> <br /> " +
                            $"N??i dung c??ng vi???c: <br />" +
                            $"{item.NoiDung}"
                    };
                    await  _notifyService.WithNotify(notify).WithRecipients(item.ThanhPhanThamDu?.Select(x => x.Id).ToList()).PushNotify();
                }

            }
            catch (Exception e)
            {
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
                    .WithMessage("Kh??ng t??m th???y c??ng vi???c ho???c l???ch c??ng t??c.");
            }
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }
            try
            {
                var listUser = new List<string>();
                if (entity.CongViecs != default)
                {
                    foreach (var item in  entity.CongViecs)
                    {
                        var notify = new Notify()
                        {
                            Title =
                                $"B???n c?? m???t l???ch c??ng t??c m???i!",
                            Content =
                                $"B???n c?? m???t l???ch c??ng t??c v??o ng??y {item.TuNgay?.ToShortDateString()} <a href='/xem-lich-cong-tac'> Xem l???ch c??ng t??c</a> <br /> " +
                                $"N??i dung c??ng vi???c: <br />" +
                                $"{item.NoiDung}"
                        };
                        await  _notifyService.WithNotify(notify).WithRecipients(item.ThanhPhanThamDu?.Select(x => x.Id).ToList()).PushNotify();
                    }
                }


            }
            catch (Exception e)
            {
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
                    .WithMessage("Kh??ng t??m th???y c??ng vi???c ho???c l???ch c??ng t??c.");
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
                    .WithMessage("Kh??ng t??m th???y l???ch c??ng t??c!");
            }

            var congViec = lichCongTac.CongViecs.Where(x => x.Id == model.Id).FirstOrDefault();
            if (congViec == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Kh??ng t??m th???y c??ng vi???c!");
            }
            return congViec;
        }

        #endregion
    }
}