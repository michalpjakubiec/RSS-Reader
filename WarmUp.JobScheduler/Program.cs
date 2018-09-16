using System;
using Autofac;
using Hangfire;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;
using Hangfire.Storage;
using WarmUp.Core;
using WarmUp.Core.DB;
using WarmUp.Core.DB.Interfaces;
using WarmUp.Core.Services;
using WarmUp.Core.Services.Interfaces;

namespace WarmUp.JobScheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateDbIfNotExist();
            AppStart();
            RemoveAllJobsFromJobStore();

            using (var server = new BackgroundJobServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1
            }))
            {
                RecurringJob.AddOrUpdate<RSSService>(x => x.UpadteData(), Cron.Minutely);
                Console.WriteLine("Hangfire JobScheduler Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static void CreateDbIfNotExist()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            new RSSContext().Configure();
        }

        private static void RemoveAllJobsFromJobStore()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
        }

        private static void AppStart()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RSSContext>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<RSSPageParser>()
                .As<IRSSPageParser>()
                .SingleInstance();
            builder.RegisterType<RSSReader>()
                .As<IRSSReader>()
                .SingleInstance();
            builder.RegisterType<UnitOfWrok>()
                .As<IUnitOfWork>()
                .PropertiesAutowired()
                .SingleInstance();
            builder.RegisterType<RSSService>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(string), "http://www.rss.lostsite.pl//index.php?rss=32"));
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>));

            LogProvider.SetCurrentLogProvider(new ColouredConsoleLogProvider());

            GlobalConfiguration.Configuration.UseAutofacActivator(builder.Build());
            GlobalConfiguration.Configuration.UseSqlServerStorage(nameof(RSSContext));
        }
    }
}