using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using QuartzSetup.Factories;
using QuartzSetup.Jobs;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;

namespace QuartzSetup.Services;

public static class DependencyInjector
{
    public static async Task<IServiceCollection> AddQuartzServicesInMemory(this IServiceCollection services)
    {

        var properties = new NameValueCollection
        {
            // 调度器名称，便于标识
            ["quartz.scheduler.instanceName"] = "QuartzSetupScheduler",

            // 实例 ID：用于集群场景标识不同节点，"AUTO" 会自动生成唯一 ID（适用于单实例和开发环境）
            ["quartz.scheduler.instanceId"] = "AUTO",

            // 最大并发数，避免线程过多影响性能（视任务量调整）
            ["quartz.threadPool.maxConcurrency"] = "5",

            // 设置 Misfire（触发失败）处理阈值，单位为毫秒
            ["quartz.jobStore.misfireThreshold"] = "60000", // 默认值 60 秒

            // 阻止 Quartz 关闭时立即停止正在运行的任务
            ["quartz.scheduler.batchTriggerAcquisitionMaxCount"] = "5", // 批量获取触发器最大数量
        };

        var scheduler = await SchedulerBuilder.Create(properties)
            .UseDefaultThreadPool(tp => tp.MaxConcurrency = 5)
            .UseInMemoryStore() // 使用内存存储，不支持持久化，适合开发和测试
            .BuildScheduler();

        await scheduler.Start();

        services.AddSingleton(scheduler);


        return services;
    }

    public static async Task<IServiceCollection> AddQuartzServicesInDb(this IServiceCollection services, IConfiguration configuration)
    {

        var quartzConfig = configuration.GetSection("Quartz").Get<QuartzConfig>();
        if (quartzConfig != null)
        {
            var properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = quartzConfig.Scheduler.InstanceName,
                ["quartz.scheduler.instanceId"] = quartzConfig.Scheduler.InstanceId,
                ["quartz.threadPool.type"] = quartzConfig.ThreadPool.Type,
                ["quartz.threadPool.maxConcurrency"] = quartzConfig.ThreadPool.MaxConcurrency.ToString(),
                ["quartz.jobStore.type"] = quartzConfig.JobStore.Type,
                ["quartz.jobStore.driverDelegateType"] = quartzConfig.JobStore.DriverDelegateType,
                ["quartz.jobStore.dataSource"] = quartzConfig.JobStore.DataSource,
                ["quartz.jobStore.tablePrefix"] = quartzConfig.JobStore.TablePrefix,
                ["quartz.jobStore.misfireThreshold"] = quartzConfig.JobStore.MisfireThreshold.ToString(),
                ["quartz.dataSource.default.connectionString"] = quartzConfig.DataSource.Default.ConnectionString,
                ["quartz.dataSource.default.provider"] = quartzConfig.DataSource.Default.Provider,
            };

            var scheduler = await SchedulerBuilder.Create(properties)
                .UsePersistentStore(x =>
                {
                    x.UseSqlServer(quartzConfig.DataSource.Default.ConnectionString);
                    x.UseNewtonsoftJsonSerializer(); // 使用 JSON 序列化作业数据
                    x.UseProperties = true; // 强制使用字符串存储数据
                })
                .UseDefaultThreadPool(tp => tp.MaxConcurrency = quartzConfig.ThreadPool.MaxConcurrency)
                .BuildScheduler();

            await scheduler.Start();

            services.AddSingleton(scheduler);

            services.AddTransient<ReminderJobFactory>();
            services.AddTransient<NotificationJobFactory>();

            services.AddSingleton<RepeatingJobFactory>();
            services.AddSingleton<SchedulerJobFactory>();

            services.AddSingleton<IJobSchedulerService, JobSchedulerService>();

        }

        return services;
    }
}

public class QuartzConfig
{
    public SchedulerConfig Scheduler { get; set; }
    public ThreadPoolConfig ThreadPool { get; set; }
    public JobStoreConfig JobStore { get; set; }
    public DataSourceConfig DataSource { get; set; }
}

public class SchedulerConfig
{
    public string InstanceName { get; set; }
    public string InstanceId { get; set; }
}

public class ThreadPoolConfig
{
    public string Type { get; set; }
    public int MaxConcurrency { get; set; }
}

public class JobStoreConfig
{
    public string Type { get; set; }
    public string DriverDelegateType { get; set; }
    public string DataSource { get; set; }
    public string TablePrefix { get; set; }
    public int MisfireThreshold { get; set; }
}

public class DataSourceConfig
{
    public DefaultConfig Default { get; set; }
}

public class DefaultConfig
{
    public string ConnectionString { get; set; }
    public string Provider { get; set; }
}
