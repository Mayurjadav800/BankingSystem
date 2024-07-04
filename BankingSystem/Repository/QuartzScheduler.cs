using BankingSystem.Repository;
using Quartz.Impl;
using Quartz;

namespace BankingSystem.Repository
{
    public class QuartzScheduler
    {
        public static async Task StartAsync(IServiceProvider serviceProvider)
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<BirthDayEmailJob>()
                .WithIdentity("birthdayEmailJob", "group1")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("birthdayEmailTrigger", "group1")
                 .WithSchedule(CronScheduleBuilder.CronSchedule("0 05 18 * * ?")) // Runs every day at 2:55 PM
                .Build();
            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
