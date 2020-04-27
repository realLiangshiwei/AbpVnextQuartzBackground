[Dependency(ReplaceServices = true)]
public class CustomQuartzBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
{
    private readonly IScheduler _scheduler;

    public CustomQuartzBackgroundWorkerManager(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _scheduler.ResumeAll(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (!_scheduler.IsShutdown)
        {
            await _scheduler.PauseAll(cancellationToken);
        }
    }

    public void Add(IBackgroundWorker worker)
    {
        if (await _scheduler.CheckExists(quartzWork.JobDetail.Key))
        {
            await _scheduler.AddJob(quartzWork.JobDetail, true, true);
            await _scheduler.ResumeJob(quartzWork.JobDetail.Key);
            await _scheduler.RescheduleJob(quartzWork.Trigger.Key, quartzWork.Trigger);
        }
        else
        {
            await _scheduler.ScheduleJob(quartzWork.JobDetail, quartzWork.Trigger);
        }
    }
}
