using AutoMapper.Data;
using credinet.comun.api;
using Domain.Model.Entities.Gateway;
using Domain.UseCase.Common;
using DrivenAdapters.Mongo;
using DrivenAdapters.Mongo.Adapters;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using Domain.UseCase.Accounts;
using Domain.UseCase.Clients;
using Domain.UseCase.Transactions;
using Domain.UseCase.Users;
using TallerBackGrupalBOT.AppServices.Automapper;

namespace TallerBackGrupalBOT.AppServices.Extensions
{
    /// <summary>
    /// Service Extensions
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers the cors.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterCors(this IServiceCollection services, string policyName) =>
            services.AddCors(o => o.AddPolicy(policyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

        /// <summary>
        /// Método para registrar AutoMapper
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(cfg => { cfg.AddDataReaderMapping(); }, typeof(ConfigurationProfile));

        /// <summary>
        /// Método para registrar Mongo
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="connectionString">connection string.</param>
        /// <param name="db">database.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterMongo(this IServiceCollection services, string connectionString,
            string db) =>
            services.AddSingleton<IContext>(provider => new Context(connectionString, db));

        /// <summary>
        /// Método para registrar Redis Cache
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="connectionString">connection string.</param>
        /// <param name="dbNumber">database number.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRedis(this IServiceCollection services, string connectionString,
            int dbNumber)
        {
            services.AddSingleton(s => LazyConnection(connectionString).Value.GetDatabase(dbNumber));

            ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(connectionString,
                opt => opt.DefaultDatabase = dbNumber);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            return services;
        }

        /// <summary>
        /// Método para registrar los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            #region Helpers

            services.AddSingleton<IMensajesHelper, MensajesApiHelper>();

            #endregion Helpers

            #region Adaptadores

            services.AddScoped<IUserRepository, UserRepositoryAdapter>();
            services.AddScoped<IAccountRepository, AccountRepositoryAdapter>();
            services.AddScoped<IClientRepository, ClientRepositoryAdapter>();
            services.AddScoped<ITransactionRepository, TransactionRepositoryAdapter>();

            #endregion Adaptadores

            #region UseCases

            services.AddScoped<IManageEventsUseCase, ManageEventsUseCase>();
            services.AddScoped<IUserUseCase, UserUseCase>();
            services.AddScoped<ICuentaUseCase, AccountUseCase>();
            services.AddScoped<IClientUseCase, ClientUseCase>();
            services.AddScoped<ITransactionUseCase, TransactionUseCase>();

            #endregion UseCases

            return services;
        }

        /// <summary>
        /// Lazies the connection.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        /// <returns></returns>
        private static Lazy<ConnectionMultiplexer> LazyConnection(string connectionString) =>
            new(() => { return ConnectionMultiplexer.Connect(connectionString); });
    }
}