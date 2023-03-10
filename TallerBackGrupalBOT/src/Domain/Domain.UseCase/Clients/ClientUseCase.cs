using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using credinet.exception.middleware.models;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Users;
using Helpers.Commons.Exceptions;

namespace Domain.UseCase.Clients
{
    /// <summary>
    /// <see cref="IClientUseCase"/>
    /// </summary>
    public class ClientUseCase : IClientUseCase
    {
        private readonly IClientRepository _gatewayClient;

        private readonly IAccountRepository _gatewayAccount;

        private readonly IUserRepository _gatewayUser;

        /// <summary>
        /// Constructor para inyectar gateway
        /// </summary>
        /// <param name="gatewayClient"></param>
        /// <param name="gatewayAccount"></param>
        /// <param name="gatewayUser"></param>
        public ClientUseCase(IClientRepository gatewayClient, IAccountRepository gatewayAccount,
            IUserRepository gatewayUser)
        {
            _gatewayClient = gatewayClient;
            _gatewayAccount = gatewayAccount;
            _gatewayUser = gatewayUser;
        }

        /// <summary>
        /// <see cref=IClientUseCase.UpdateEmail)"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Client> UpdateEmail(string clientId, string newEmail)
        {
            var clienteSeleccionado = await _gatewayClient.FindByIdAsync(clientId);

            if (clienteSeleccionado is null)
                throw new BusinessException($"No existe client con el id {clientId}",
                    (int)TipoExcepcionNegocio.ClienteNoExiste);

            clienteSeleccionado.UpdateEmail(newEmail);

            if (!clienteSeleccionado.CheckEmail())
                throw new BusinessException($"Correo electrónico nuevo no valido",
                    (int)TipoExcepcionNegocio.CorreoElectronicoNoValido);

            return await _gatewayClient.UpdateAsync(clientId, clienteSeleccionado);
        }

        /// <summary>
        /// <see cref=IClientUseCase.AddProductToClient)"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Client> AddProductToClient(string clientId, Account newAccount)
        {
            var clienteSeleccionado = await _gatewayClient.FindByIdAsync(clientId);

            if (clienteSeleccionado is null)
                throw new BusinessException($"No existe client con el id {clientId}",
                    (int)TipoExcepcionNegocio.ClienteNoExiste);

            clienteSeleccionado.AddProductId(newAccount.Id);

            return await _gatewayClient.UpdateAsync(clientId, clienteSeleccionado);
        }

        /// <summary>
        /// <see cref=IClientUseCase.CreateClient)"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newClient"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Client> CreateClient(string userId, Client newClient)
        {
            var clienteVerificacion = await _gatewayClient.FindByIdNumber(newClient.IdNumber);

            User userSeleccionado = await _gatewayUser.FindByIdAsync(userId);

            if (userSeleccionado is null)
                throw new BusinessException($"No se encontró un user para la creación",
                    (int)TipoExcepcionNegocio.UsuarioNoValido);

            if (userSeleccionado.Rol != Roles.Admin)
                throw new BusinessException($"El user {userSeleccionado.FullName} no puede crear nuevos clientes",
                    (int)TipoExcepcionNegocio.UsuarioNoValido);

            if (clienteVerificacion is not null)
                throw new BusinessException($"Client con numero de identificación {newClient.IdNumber} ya existe",
                    (int)TipoExcepcionNegocio.IdentificacionDeClienteYaExiste);

            if (!newClient.VerifyClientAge(LegalAge.col))
                throw new BusinessException($"El client debe ser mayor de edad",
                    (int)TipoExcepcionNegocio.ClienteNoEsMayorDeEdad);

            if (!newClient.CheckEmail())
                throw new BusinessException($"El correo electrónico {newClient.Email} no es valido",
                    (int)TipoExcepcionNegocio.CorreoElectronicoNoValido);

            HistoryUpdate nuevaActualizacion = new(HistoryUpdateType.Creation, userSeleccionado);

            newClient.AddHistoryUpdate(nuevaActualizacion);

            newClient.Enable();
            newClient.CreationDate = DateTime.Now;

            return await _gatewayClient.CreateAsync(userSeleccionado.Id, newClient);
        }

        /// <summary>
        /// <see cref=IClientUseCase.DisableClient)"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> DisableClient(string clientId)
        {
            var clienteSeleccionado = await _gatewayClient.FindByIdAsync(clientId);

            if (clienteSeleccionado is null)
                throw new BusinessException($"No existe client con el id {clientId}",
                    (int)TipoExcepcionNegocio.ClienteNoExiste);

            if (clienteSeleccionado.HasActiveDebts)
                throw new BusinessException($"Client no es posible deshabilitar por deudas activas",
                    (int)TipoExcepcionNegocio.ClienteTieneDeudasActivas);

            clienteSeleccionado.Disable();
            await _gatewayClient.UpdateAsync(clientId, clienteSeleccionado);

            return true;
        }

        /// <summary>
        /// <see cref=IClientUseCase.DisableClientDebt)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> DisableClientDebt(string idCliente)
        {
            var clienteSeleccionado = await _gatewayClient.FindByIdAsync(idCliente);

            if (clienteSeleccionado is null)
                throw new BusinessException($"No existe client con el id {idCliente}",
                    (int)TipoExcepcionNegocio.ClienteNoExiste);

            clienteSeleccionado.DisableDebt();
            await _gatewayClient.UpdateAsync(idCliente, clienteSeleccionado);

            return true;
        }

        /// <summary>
        /// <see cref=IClientUseCase.EnableClient)"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> EnableClient(string clientId)
        {
            var clienteSeleccionado = await _gatewayClient.FindByIdAsync(clientId);

            if (clienteSeleccionado is null)
                throw new BusinessException($"No existe client con el id {clientId}",
                    (int)TipoExcepcionNegocio.ClienteNoExiste);

            clienteSeleccionado.Enable();
            await _gatewayClient.UpdateAsync(clientId, clienteSeleccionado);

            return true;
        }

        /// <summary>
        /// <see cref=IClientUseCase.EnableClientDebt)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> EnableClientDebt(string idCliente)
        {
            var clienteSeleccionado = await _gatewayClient.FindByIdAsync(idCliente);

            if (clienteSeleccionado is null)
                throw new BusinessException($"No existe client con el id {idCliente}",
                    (int)TipoExcepcionNegocio.ClienteNoExiste);

            clienteSeleccionado.EnableDebt();
            await _gatewayClient.UpdateAsync(idCliente, clienteSeleccionado);

            return true;
        }

        /// <summary>
        /// <see cref=IClientUseCase.FindClientById)"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> FindClientById(string clientId) =>
            await _gatewayClient.FindByIdAsync(clientId);

        /// <summary>
        /// <see cref=IClientUseCase.FindAllFindAll"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Client>> FindAll() =>
            await _gatewayClient.FindAllAsync();
    }
}