using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using credinet.exception.middleware.models;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Users;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils;
using Helpers.ObjectsUtils.Extensions;
using Microsoft.Extensions.Options;

namespace Domain.UseCase.Accounts
{
    /// <summary>
    /// Cuenta UseCase
    /// </summary>
    public class AccountUseCase : ICuentaUseCase
    {
        private readonly IAccountRepository _repositoryAccount;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;

        private readonly IOptions<ConfiguradorAppSettings> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountUseCase"/> class.
        /// </summary>
        /// <param name="repositoryAccount">The logger.</param>
        /// <param name="clientRepository">The logger.</param>
        /// <param name="userRepository">The logger.</param>
        public AccountUseCase(IAccountRepository repositoryAccount, IClientRepository clientRepository, IUserRepository userRepository, IOptions<ConfiguradorAppSettings> options)
        {
            _repositoryAccount = repositoryAccount;
            _clientRepository = clientRepository;
            _userRepository = userRepository;

            _options = options;
        }

        /// <summary>
        /// <see cref="ICuentaUseCase.CancelAccount"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> CancelAccount(string userId, Account account)
        {
            var usuario = await _userRepository.FindByIdAsync(userId);
            var cuentaEncontrada = await _repositoryAccount.FindByIdAsync(account.Id);

            if (usuario is null)
                throw new BusinessException(TipoExcepcionNegocio.UsuarioNoExiste.GetDescription(),
                               (int)TipoExcepcionNegocio.UsuarioNoExiste);

            if (cuentaEncontrada is null)
                throw new BusinessException(TipoExcepcionNegocio.CuentaNoEncontrada.GetDescription(),
                               (int)TipoExcepcionNegocio.CuentaNoEncontrada);

            if (usuario.Rol.Equals(Roles.Transactional))
                throw new BusinessException(TipoExcepcionNegocio.UsuarioSinPermisos.GetDescription(),
                                (int)TipoExcepcionNegocio.UsuarioSinPermisos);

            if (cuentaEncontrada.Balance >= 1)
                throw new BusinessException(TipoExcepcionNegocio.CuentaConSaldo.GetDescription(),
                                                   (int)TipoExcepcionNegocio.CuentaConSaldo);

            Modification nuevaModificacion = new Modification(ModificationType.Cancellation, usuario);
            cuentaEncontrada.CancelAccount();
            cuentaEncontrada.AddModification(nuevaModificacion);
            return await _repositoryAccount.UpdateAsync(cuentaEncontrada.Id, cuentaEncontrada);
        }

        /// <summary>
        /// <see cref="ICuentaUseCase.DisableAccount"/>
        /// </summary>
        /// <param name="idUsuarioModificacion"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> DisableAccount(string idUsuarioModificacion, Account account)
        {
            var usuario = await _userRepository.FindByIdAsync(idUsuarioModificacion);
            var cuentaEncontrada = await _repositoryAccount.FindByIdAsync(account.Id);
            if (usuario == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.UsuarioNoExiste.GetDescription(),
                               (int)TipoExcepcionNegocio.UsuarioNoExiste);
            }
            else if (cuentaEncontrada == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.CuentaNoEncontrada.GetDescription(),
                               (int)TipoExcepcionNegocio.CuentaNoEncontrada);
            }
            else if (usuario.Rol.Equals(Roles.Transactional))
            {
                throw new BusinessException(TipoExcepcionNegocio.UsuarioSinPermisos.GetDescription(),
                                (int)TipoExcepcionNegocio.UsuarioSinPermisos);
            }
            else if (cuentaEncontrada.AccountStatus.Equals(AccountStatus.Inactive))
            {
                throw new BusinessException(TipoExcepcionNegocio.CuentaEstaInactiva.GetDescription(),
                                (int)TipoExcepcionNegocio.CuentaEstaInactiva);
            }
            Modification nuevaModificacion = new Modification(ModificationType.Disable, usuario);
            cuentaEncontrada.DisableAccount();
            cuentaEncontrada.AddModification(nuevaModificacion);
            return await _repositoryAccount.UpdateAsync(cuentaEncontrada.Id, cuentaEncontrada);
        }

        /// <summary>
        /// <see cref="ICuentaUseCase.EnableAccount"/>
        /// </summary>
        /// <param name="idUsuarioModificacion"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> EnableAccount(string idUsuarioModificacion, Account account)
        {
            var usuario = await _userRepository.FindByIdAsync(idUsuarioModificacion);
            var cuentaEncontrada = await _repositoryAccount.FindByIdAsync(account.Id);
            if (usuario == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.UsuarioNoExiste.GetDescription(),
                               (int)TipoExcepcionNegocio.UsuarioNoExiste);
            }
            else if (cuentaEncontrada == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.CuentaNoEncontrada.GetDescription(),
                               (int)TipoExcepcionNegocio.CuentaNoEncontrada);
            }
            else if (usuario.Rol.Equals(Roles.Transactional))
            {
                throw new BusinessException(TipoExcepcionNegocio.UsuarioSinPermisos.GetDescription(),
                                (int)TipoExcepcionNegocio.UsuarioSinPermisos);
            }
            else if (cuentaEncontrada.AccountStatus.Equals(AccountStatus.Active))
            {
                throw new BusinessException(TipoExcepcionNegocio.CuentaEstaActiva.GetDescription(),
                                (int)TipoExcepcionNegocio.CuentaEstaActiva);
            }
            Modification nuevaModificacion = new Modification(ModificationType.Enable, usuario);
            cuentaEncontrada.EnableAccount();
            cuentaEncontrada.AddModification(nuevaModificacion);
            return await _repositoryAccount.UpdateAsync(cuentaEncontrada.Id, cuentaEncontrada);
        }

        /// <summary>
        /// <see cref="ICuentaUseCase.FindAccountById"/>
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        public async Task<Account> FindAccountById(string idCuenta)
        {
            var cuenta = await _repositoryAccount.FindByIdAsync(idCuenta);
            if (cuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.CuentaNoEncontrada.GetDescription(),
                               (int)TipoExcepcionNegocio.CuentaNoEncontrada);
            }
            return cuenta;
        }

        /// <summary>
        /// Método para crear una Cuenta
        /// </summary>
        /// <param name="idUsuarioModificacion"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> Create(string idUsuarioModificacion, Account account)
        {
            if (!account.Exempt) { account.CalculateAvailableBalance(_options.Value.GMF); }
            else { account.AvailableBalance = account.Balance; }
            account.AssignAccountNumber();

            var cuentasCliente = await _repositoryAccount.FindByClientAsync(account.ClientId);
            var cuentasExentas = cuentasCliente.Where(x => x.Exempt).ToList();

            var usuario = await _userRepository.FindByIdAsync(idUsuarioModificacion);
            var cliente = await _clientRepository.FindByIdAsync(account.ClientId);
            if (usuario == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.UsuarioNoExiste.GetDescription(),
                               (int)TipoExcepcionNegocio.UsuarioNoExiste);
            }
            else if (cliente == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ClienteNoExiste.GetDescription(),
                               (int)TipoExcepcionNegocio.ClienteNoExiste);
            }
            else if (usuario.Rol.Equals(Roles.Transactional))
            {
                throw new BusinessException(TipoExcepcionNegocio.UsuarioSinPermisos.GetDescription(),
                                (int)TipoExcepcionNegocio.UsuarioSinPermisos);
            }
            else if (cuentasExentas.Count > 0 && account.Exempt)
            {
                throw new BusinessException(TipoExcepcionNegocio.YaExisteUnaCuentaExenta.GetDescription(),
                                                  (int)TipoExcepcionNegocio.YaExisteUnaCuentaExenta);
            }

            var nuevaModificacion = new Modification(ModificationType.Creation, usuario);
            var nuevaActualizacion = new HistoryUpdate(HistoryUpdateType.Update, usuario);
            account.AddModification(nuevaModificacion);
            cliente.AddProductId(account.AccountNumber);
            cliente.AddHistoryUpdate(nuevaActualizacion);
            await _clientRepository.UpdateAsync(cliente.Id, cliente);
            return await _repositoryAccount.CreateAsync(account);
        }

        /// <summary>
        /// Método para obtener todas la cuentas
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> FindAll()
        {
            return await _repositoryAccount.FindAllAsync();
        }

        /// <summary>
        /// Método para obtener todas la cuentas por Client
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> FindAllByClient(string clientId)
        {
            var cliente = await _clientRepository.FindByIdAsync(clientId);
            if (cliente == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ClienteNoExiste.GetDescription(),
                               (int)TipoExcepcionNegocio.ClienteNoExiste);
            }

            var cuentasCliente = await _repositoryAccount.FindByClientAsync(clientId);

            //Ordenar Cuentas por Balance
            return cuentasCliente.OrderByDescending(x => x.Balance).ToList();
        }
    }
}