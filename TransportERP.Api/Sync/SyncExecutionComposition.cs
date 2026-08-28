using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

public static class SyncExecutionComposition
{
    public static IServiceCollection AddSyncBusinessExecution(
        this IServiceCollection services,
        bool workerEnabled)
    {
        services.AddScoped<ISyncWaybillBusinessAdapter, SyncWaybillBusinessAdapter>();
        services.AddScoped<ISyncFinanceBusinessAdapter, SyncFinanceBusinessAdapter>();
        services.AddScoped<ISyncShippingBusinessAdapter, SyncShippingBusinessAdapter>();
        services.AddScoped<ISyncBusinessDispatchAuditSink, SyncBusinessDispatchAuditSink>();
        services.AddScoped<SyncBusinessDispatcher>();
        services.AddScoped<ISyncActionExecutor, SyncBusinessActionExecutor>();
        services.AddScoped<SyncExecutionProcessor>();
        if (workerEnabled)
            services.AddHostedService<SyncExecutionWorker>();
        return services;
    }
}
