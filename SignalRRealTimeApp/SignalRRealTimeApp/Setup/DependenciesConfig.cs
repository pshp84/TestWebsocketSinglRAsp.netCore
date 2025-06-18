using NuGet.Protocol.Core.Types;
using SignalRRealTimeApp.DAL.IRepository;
using SignalRRealTimeApp.DAL.IServices;
using SignalRRealTimeApp.DAL.Repository;
using SignalRRealTimeApp.DAL.Services;

namespace SignalRRealTimeApp.Setup
{
    public class DependenciesConfig
    {
        public static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddTransient<IChatService,ChatService>();
            services.AddTransient<IChatTrackingService, ChatTrackingService>();
            services.AddTransient<IErrorLogService, ErrorLogService>();
            services.AddTransient<ILiveLogService, LiveLogService>();
            services.AddTransient<IGroupService, GroupService>();


            services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();

        }
    }
}
