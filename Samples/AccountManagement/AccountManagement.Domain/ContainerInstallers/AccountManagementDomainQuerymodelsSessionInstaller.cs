﻿using AccountManagement.Domain.Events.EventStore.ContainerInstallers;
using AccountManagement.Domain.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Composable.KeyValueStorage;
using Composable.KeyValueStorage.SqlServer;
using Composable.System.Configuration;
using JetBrains.Annotations;

namespace AccountManagement.Domain.ContainerInstallers
{
    [UsedImplicitly]
    public class AccountManagementDomainQuerymodelsSessionInstaller : IWindsorInstaller
    {
        public static class ComponentKeys
        {
            public const string KeyForDocumentDb = "AccountManagement.Domain.QueryModels.IDocumentDb";
            public const string KeyForInMemoryDocumentDb = "AccountManagement.Domain.QueryModels.IDocumentDb.InMemory";
            public const string KeyForSession = "AccountManagement.Domain.QueryModels.IDocumentDbSession";
        }

        public static string ConnectionStringName { get { return AccountManagementDomainEventStoreInstaller.ConnectionStringName; } }

        public void Install(
            IWindsorContainer container,
            IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentDb>()
                    .ImplementedBy<SqlServerDocumentDb>()
                    .DependsOn(new {connectionString = GetConnectionStringFromConfiguration(ConnectionStringName)})
                    .Named(ComponentKeys.KeyForDocumentDb)
                    .LifestylePerWebRequest(),
                Component.For<IDocumentDbSession, IAccountManagementDomainQueryModelSession>()
                    .ImplementedBy<AccountManagementDomainQueryModelSession>()
                    .DependsOn(
                        Dependency.OnComponent(typeof(IDocumentDb), ComponentKeys.KeyForDocumentDb),
                        Dependency.OnValue<IDocumentDbSessionInterceptor>(NullOpDocumentDbSessionInterceptor.Instance))
                    .Named(ComponentKeys.KeyForSession)
                    .LifestylePerWebRequest()
                );
        }

        private static string GetConnectionStringFromConfiguration(string key)
        {
            return new ConnectionStringConfigurationParameterProvider().GetConnectionString(key).ConnectionString;
        }
    }
}