using Quartz;
using QuartzSetup.Jobs.JobRequest;

namespace QuartzSetup.Factories;

public interface IJobFactory
{
    IJobDetail CreateJob();
    ITrigger CreateTrigger();
    IJobDetail CreateJob(IJobRequest request);
    ITrigger CreateTrigger(IJobRequest request);
}
