using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EOffice.WebAPI.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}