using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Interfaces.Identity;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Services;

namespace EOffice.WebAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
            services.AddSingleton<IDbSettings>(sp => sp.GetRequiredService<IOptions<DbSettings>>().Value);
            services.AddSingleton<DataContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDefineStatusService,DefineStatusService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IDonViService, DonViService>();
            services.AddScoped<IChucVuService, ChucVuService>();
            services.AddScoped<ILinhVucService, LinhVucService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IDMHanhChinhService, DMHanhChinhService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<HistoryQuestionService>();
            services.AddScoped<IWarningService, WarningService>();
            services.AddScoped<ITrangThaiService, TrangThaiService>();
            services.AddScoped<IModuleTrangThaiService, ModuleTrangThaiService>();
            services.AddScoped<ILoaiVanBanService, LoaiVanBanService>();
            services.AddScoped<IHoSoDonViService, HoSoDonViService>();
        }
    }
}