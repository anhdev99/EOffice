using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using EOffice.WebAPI.ViewModels;
using ExcelDataReader;
using MongoDB.Bson;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.Services
{
    public class UserService : BaseService, IUserService
    {
        private DataContext _context;
        private BaseMongoDb<User, string> BaseMongoDb;
        IMongoCollection<User> _collectionUser;
        private IDonViService _donViService;
        private ILoggingService _logger;
        private IDbSettings _settings;
        private INotifyService _notifyService;
        public UserService(
            ILoggingService logger,
            IDbSettings settings,
            IDonViService donViService,
            DataContext context,
            INotifyService notifyService,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<User, string>(_context.Users);
            _collectionUser = context.Users;
            _donViService = donViService;
            _notifyService = notifyService;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.UserCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.Find(x => x.DonVi != null && x.IsDeleted != true).SortByDescending(x => x.DonVi.Ten).ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await _context.Users.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }
        
        public async Task<User> GetUserByIdDonVi(string id)
        {
            return await _context.Users.Find(x => x.DonVi.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<User>> GetPaging(PagingParam param)
        {
            PagingModel<User> result = new PagingModel<User>();
            var builder = Builders<User>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.FullName.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(User.CreatedAt);
            result.TotalRows = await _context.Users.CountDocumentsAsync(filter);
            result.Data = await _context.Users.Find(filter)
                .Sort(param.SortDesc
                ?Builders<User>
                    .Sort.Ascending(sortBy)
                : Builders<User>
                        .Sort.Descending(sortBy)
                     )
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await _context.Users.Find(x => x.UserName == userName && x.IsDeleted != true).FirstOrDefaultAsync();
        }
        public async Task ReadDataUser(string filePath)
        {
            var donVis = _context.DonVis.Find(x => x.IsDeleted != true).ToList();
            var chucVus = _context.ChucVu.Find(x => x.IsDeleted != true).ToList();
            var roles = _context.Roles.Find(x => x.IsDeleted != true).ToList();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        int khoiCoQuanId = 0;
                        var check = reader.GetValue(7)?.ToString();
                        if (check != default)
                        {
                            var user = new User()
                            {
                                EOfficeId =  reader.GetValue(0)?.ToString(),
                                LastName = reader.GetValue(1)?.ToString(),
                                FirstName = reader.GetValue(2)?.ToString(),
                                Email = reader.GetValue(3)?.ToString(),
                                UserName =  reader.GetValue(7)?.ToString(),
                            };
                            var donVi = donVis.Where(x => x.Ten.ToLower() == reader.GetValue(4)?.ToString().ToLower()).FirstOrDefault();
                            if (donVi != default)
                            {
                                user.DonVi = donVi;
                            }
                            var chucVu = chucVus.Where(x => x.Ten.ToLower() == reader.GetValue(5)?.ToString().ToLower()).FirstOrDefault();
                            if (chucVu != default)
                            {
                                user.ChucVu = chucVu;
                            }
                            var role = roles.Where(x => x.Code == "5555").FirstOrDefault();
                            if (role != default)
                            {
                                if (user.Roles == default)
                                    user.Roles = new List<Role>();
                                user.Roles.Add(role);
                            }

                            try
                            {
                                Create(user);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                }
            }

        }
        public async Task<User> Create(User model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var checkName = _context.Users.Find(x => x.UserName == model.UserName && x.IsDeleted != true)
                .FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Tên tài khoản đã tồn tại.");
            }

            var entity = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.LastName + " " +model.FirstName,
                EOfficeId = model.EOfficeId,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Note = model.Note,
                Roles = model.Roles,
                DonVi = model.DonVi,
                ChucVu = model.ChucVu,
                IsVerified = model.IsVerified,
                IsSyncPasswordSuccess = model.IsSyncPasswordSuccess,
                IsActived = model.IsActived,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            
            // Tao5 khoa
            SignDigitalService.GeneratSignKey(ref entity);
            
            if (model.DonVi != default)
            {
                entity.DonVi = model.DonVi;
                entity.DonViIds = _donViService.GetListDonViId(model.DonVi.Id);
            }

            byte[] passwordHash, passwordSalt;
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = "DThU@123";
            }
            PasswordExtensions.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            await _logger.WithAction(nameof(this.Create)).WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog($"Tạo tài khoản: {entity.UserName}").SaveChanges();

            return entity;
        }

        public async Task<User> Update(User model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.Users.Find(x => x.Id == model.Id).FirstOrDefault();

            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var checkName = _context.Users
                .Find(x => x.Id != model.Id && x.UserName == model.UserName && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Tên tài khoản đã tồn tại");

            entity.UserName = model.UserName;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.FullName = model.LastName + " " + model.FirstName;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Email = model.Email;
            entity.Note = model.Note;
            entity.KySo = model.KySo;
            entity.Roles = model.Roles;
            entity.      DonVi = model.DonVi;
            entity.       ChucVu = model.ChucVu;
            entity.ModifiedAt = DateTime.Now;
            entity.IsVerified = model.IsVerified;
            entity.IsSyncPasswordSuccess = model.IsSyncPasswordSuccess;
            entity.IsActived = model.IsActived;
            
            if (model.DonVi != default)
            {
                if (model.DonVi.Id != entity.DonVi.Id)
                {
                    entity.DonVi = model.DonVi;
                    entity.DonViIds = _donViService.GetListDonViId(model.DonVi.Id);
                }
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordExtensions.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = passwordSalt;
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _logger.WithAction(nameof(this.Update))
                .WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog(DefaultMessage.UPDATE_SUCCESS)
                .SaveChanges();
            return entity;
        }

        public async Task<User> ChangeProfile(User model)
        {
            if (model == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.Users.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.FullName = model.LastName + " " + model.FirstName;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Email = model.Email;
            entity.Avatar = model.Avatar;
            entity.KySo = model.KySo;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Cập nhật thông tin thất bại.");
            }

            await _logger.WithAction(nameof(this.ChangeProfile))
                .WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog("Cập nhật thông tin thành công.")
                .SaveChanges();
            return entity;
        }

        public async Task<User> ChangePassword(UserVM model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.Users.Find(x => x.UserName == model.UserName).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] passHash, passSalt;
                passHash = entity.PasswordHash;
                passSalt = entity.PasswordSalt;
                var pass = PasswordExtensions.VerifyPasswordHash(model.Password, passHash, passSalt);
                if (pass != true)
                {
                    throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Mật khẩu không chính xác");
                }
                else
                {
                    if (model.newPass == model.confirmPass)
                    {
                        byte[] passwordHash, passwordSalt;
                        PasswordExtensions.CreatePasswordHash(model.newPass, out passwordHash, out passwordSalt);
                        entity.PasswordHash = passwordHash;
                        entity.PasswordSalt = passwordSalt;
                    }
                }
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Đổi mật khẩu thất bại.");
            }

            await _logger.WithAction(nameof(this.ChangePassword))
                .WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog("Đổi mật khẩu thành công.")
                .SaveChanges();
            return entity;
        }

        public async Task Delete(string id)
        {
            if (id == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.Users.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            entity.ModifiedAt = DateTime.Now;
            entity.IsDeleted = true;

            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }

            await _logger.WithAction(nameof(this.Delete))
                .WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog(DefaultMessage.DELETE_SUCCESS)
                .SaveChanges();
        }

        public async Task<User> FindUserWithUserNameOrPhoneNumber(string input)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Where(q => q.UserName.Equals(input) || q.PhoneNumber.Equals(input));

            var query = _context.Users.Find(filter);
            return await query.FirstOrDefaultAsync();
        }
        
        
    }
}
