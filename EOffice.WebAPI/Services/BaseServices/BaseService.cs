using System.Linq;
using Microsoft.AspNetCore.Http;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Models;
using MongoDB.Driver;

namespace EOffice.WebAPI.Services
{
    public abstract class BaseService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _context;
        //protected readonly ApplicationDbContext context;
        private HttpContext _httpContext { get { return _contextAccessor.HttpContext; } }
        private User _appUser;
        protected User CurrentUser => GetCurrentUser();
        protected UserShort CurrentUserShort => GetCurrentUserShort();
        protected string CurrentUserName => GetCurrentUser()?.UserName;
        
        protected string IdUserName=> GetCurrentUser()?.Id;
        
        protected string IdDonVi=> GetCurrentUser()?.DonVi.Id;

        public BaseService(DataContext context,
            IHttpContextAccessor contextAccessor)
        {
            this._context = context;
            this._contextAccessor = contextAccessor;
        }

        private User GetCurrentUser()
        {
            // if (!_httpContext.User.Identity.IsAuthenticated)
            //     return new User();

            if (_appUser != null)
                return _appUser;

            var userId = _httpContext.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            _appUser = _context.Users.FindAsync(x => x.Id == userId).Result.FirstOrDefault();
            return _appUser;
        }
        
        private UserShort GetCurrentUserShort()
        {
            // if (!_httpContext.User.Identity.IsAuthenticated)
            //     return new User();
            

            var userId = _httpContext.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            var userShort = _context.Users.AsQueryable().Where(x => x.Id == userId).Select(x => new UserShort()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                DonVi = x.DonVi,
                ChucVu = x.ChucVu,
            }).FirstOrDefault();
            return userShort;
        }
    }
}