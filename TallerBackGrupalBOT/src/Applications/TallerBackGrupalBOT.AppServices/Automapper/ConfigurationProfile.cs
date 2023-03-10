using AutoMapper;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clientes;
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

            CreateMap<User, UsuarioEntity>().ReverseMap();
            CreateMap<Transaction, TransacciónEntity>().ReverseMap();
            CreateMap<Client, ClienteEntity>().ReverseMap();
            CreateMap<Account, CuentaEntity>().ReverseMap();

            #endregion Domain Models to Mongo Documents

            #region Domain Models To REST Handlers

            CreateMap<User, UsuarioHandler>();

            CreateMap<Transaction, TransacciónHandler>().ReverseMap();

            CreateMap<Account, CuentaHandler>().ReverseMap();

            #endregion Domain Models To REST Handlers

            #region REST Commands to Domain Models

            CreateMap<CrearUsuario, User>();

            CreateMap<CrearCliente, Client>();

            CreateMap<CrearTransacción, Transaction>();

            CreateMap<Account, CuentaHandler>().ReverseMap();

            CreateMap<CrearCuenta, Account>();

            CreateMap<EstadosCuenta, Account>();

            #endregion REST Commands to Domain Models

            #region GRPc command to HTTP command

            CreateMap<CrearUsuarioRequest, CrearUsuario>();
            CreateMap<CrearClienteProto, CrearCliente>();

            #endregion GRPc command to HTTP command

            #region GRPc command to GRPc DTO

            CreateMap<CrearUsuarioRequest, CrearUsuarioProto>().ReverseMap();
            CreateMap<HistoryUpdate, ActualizacionProto>().ReverseMap();

            #endregion GRPc command to GRPc DTO

            #region GRPc DTO to Domain Model

            CreateMap<CrearUsuarioProto, User>().ReverseMap();
            CreateMap<CrearClienteProto, Client>().ReverseMap();

            #endregion GRPc DTO to Domain Model

            #region Domain Model to GRPc Model

            CreateMap<User, UsuarioProto>().ReverseMap();
            CreateMap<Client, ClienteProto>().ReverseMap();

            #endregion Domain Model to GRPc Model
        }
    }
}