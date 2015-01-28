﻿using Alarmy.Common;
using Alarmy.Controllers;
using Alarmy.Services;
using Alarmy.Views;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alarmy.Infrastructure
{
    public class WindsorInstaller : IWindsorInstaller
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {            
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("databasePath", typeof(DatabasePathLayoutRenderer));
            container.AddFacility<LoggingFacility>(f => f.UseNLog().WithAppConfig());
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));   

            container.Register(Component.For<Settings>().ImplementedBy<CommandLineArgsSettings>());

            container.Register(Component
                .For<IAlarmRepository>()
                .ImplementedBy<FileAlarmRepository>()
                .LifestyleSingleton()
                .DynamicParameters((k, d) => d["path"] = k.Resolve<Settings>().AlarmDatabasePath)
            );

            container.Register(Component
                .For<IAlarmService>()
                .ImplementedBy<AlarmService>()
                .LifestyleSingleton()
            );

            container.Register(Component
                .For<ITimer>()
                .ImplementedBy<Timer>()
                .LifestyleSingleton()
            );

            container.Register(Component
                .For<ISmartAlarmController>()
                .ImplementedBy<SmartAlarmController>()
                .LifestyleSingleton()
            );

            container.Register(Component
                .For<IMainView, MainForm>()
                .ImplementedBy<MainForm>()
                .LifestyleSingleton()
            );

            container.Register(Component
                .For<IAlarmFactory>()
                .ImplementedBy<AlarmFactory>()
                .LifestyleSingleton()
            );

            container.Register(Component
                .For<IDateTimeProvider>()
                .ImplementedBy<DateTimeProvider>()
                .LifestyleSingleton()
            );

            container.Register(Classes.FromThisAssembly().IncludeNonPublicTypes().BasedOn<IShowAlarmCondition>().WithServiceFirstInterface());

            container.Register(Component.For<MainViewController>().LifestyleSingleton());

            container.Register(Component.For<Program.MainFormContext>().LifestyleSingleton());
        }
    }
}
