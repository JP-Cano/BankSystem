using AutoMapper;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Transactions;
using Domain.Model.Entities.Users;
using DrivenAdapters.Mongo.entities;
using DrivenAdapters.Mongo.Entities;
using EntryPoints.GRPc.Dtos;
using EntryPoints.GRPc.Protos;
using EntryPoints.ReactiveWeb.Entities.Commands;
using EntryPoints.ReactiveWeb.Entities.Handlers;

namespace TallerBackGrupalBOT.AppServices.Automapper
{
    /// <summary>
    /// EntityProfile
    /// </summary>
    public class ConfigurationProfile : Profile
    {
        /// <summary>
        /// ConfigurationProfile
        /// </summary>
        public ConfigurationProfile()
        {
            #region Domain Models to Mongo Documents

            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<Transaction, TransactionEntity>().ReverseMap();
            CreateMap<Client, ClientEntity>().ReverseMap();
            CreateMap<Account, AccountEntity>().ReverseMap();

            #endregion Domain Models to Mongo Documents

            #region Domain Models To REST Handlers

            CreateMap<User, UserHandler>();

            CreateMap<Transaction, TransactionHandler>().ReverseMap();

            CreateMap<Account, CuentaHandler>().ReverseMap();

            #endregion Domain Models To REST Handlers

            #region REST Commands to Domain Models

            CreateMap<CreateUser, User>();

            CreateMap<CreateClient, Client>();

            CreateMap<CreateTransaction, Transaction>();

            CreateMap<Account, CuentaHandler>().ReverseMap();

            CreateMap<CreateAccount, Account>();

            CreateMap<AccountState, Account>();

            #endregion REST Commands to Domain Models

            #region GRPc command to HTTP command

            CreateMap<CreateUserRequest, CreateUser>();
            CreateMap<CreateClientRequest, CreateClient>();

            #endregion GRPc command to HTTP command

            #region GRPc command to GRPc DTO

            CreateMap<CreateUserRequest, CreateUserProto>().ReverseMap();
            CreateMap<HistoryUpdate, ModificationProto>().ReverseMap();

            #endregion GRPc command to GRPc DTO

            #region GRPc DTO to Domain Model

            CreateMap<CreateUserProto, User>().ReverseMap();
            CreateMap<CreateClientProto, Client>().ReverseMap();

            #endregion GRPc DTO to Domain Model

            #region Domain Model to GRPc Model

            CreateMap<User, UserProto>().ReverseMap();
            CreateMap<Client, ClientProto>().ReverseMap();

            #endregion Domain Model to GRPc Model
        }
    }
}