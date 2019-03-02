using System.Reflection;
using Autofac;
using Dutil.Core.Impl;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;
using GameSharp.Core.Impl;
using GameSharp.Tests.Abstract;
using GameSharp.Tests.Fakes;
using GameSharp.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using Xunit.Ioc.Autofac;
using Xunit.Sdk;

[assembly: TestFramework("GameSharp.Tests.Module.AutoFacTestConfiguration", "GameSharp.Tests")]
namespace GameSharp.Tests.Module
{
    public class AutoFacTestConfiguration : AutofacTestFramework
    {
        private const string TestSuffixConvention = "Tests";

        public AutoFacTestConfiguration(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            //Register xunit tests
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith(TestSuffixConvention));

            builder.Register(context => new TestOutputHelper())
                .AsSelf()
                .As<ITestOutputHelper>()
                .InstancePerLifetimeScope();

            //Register GameSharp dbcontext and modules, and types + Dutil Modules
            builder
                .Register(t =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<GameSharpDbContext>()
                        .UseSqlite(new SqliteConnection("DataSource=:memory:"));
                    optionsBuilder.ConfigureWarnings(configurationBuilder =>
                    {
                        configurationBuilder.Default(WarningBehavior.Ignore);
                        configurationBuilder.Log(RelationalEventId.AmbientTransactionWarning);
                        configurationBuilder.Ignore(RelationalEventId.AmbientTransactionWarning);
                    });
                    var context = new GameSharpDbContext(optionsBuilder.Options);
                    return context;
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .OnActivated(async args =>
                {
                    var db = args.Instance;

                    await db.Database.OpenConnectionAsync();
                    await db.Database.EnsureCreatedAsync();
                }).OnRelease(context => { context.Database.CloseConnection(); });

            builder.RegisterType<FakePlayerProvider>()
                .As<IPlayerProvider>()
                .As<IFakePlayerProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DawlinUtilModule>();
            builder.RegisterModule<GameSharpModule>();

            builder.RegisterType<PlayerServiceSeedHelper>()
                .AsImplementedInterfaces();
            builder.RegisterType<BackgroundHelper>()
                .AsImplementedInterfaces();

            builder.RegisterType<FakeGameDataServices>().AsImplementedInterfaces();
            builder.RegisterType<FakeGameDataServices.FakeRandomizer>()
                .AsImplementedInterfaces();

            Container = builder.Build();
        }
    }
}