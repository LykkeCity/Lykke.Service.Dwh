using Autofac;
using Lykke.Service.Dwh.Core.Services;
using Lykke.Service.Dwh.Services;
using Lykke.Service.Dwh.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Dwh.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Do not register entire settings in container, pass necessary settings to services which requires them

            builder.RegisterType<SqlAdapter>()
                .As<ISqlAdapter>()
                .SingleInstance()
                .WithParameter("connectionString", _appSettings.CurrentValue.DwhService.SqlConnectionString);
        }
    }
}
