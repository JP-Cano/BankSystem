using AutoMapper;
using Domain.UseCase.Common;
using EntryPoints.ReactiveWeb.Base;
using EntryPoints.ReactiveWeb.Entities.Commands;
using EntryPoints.ReactiveWeb.Entities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;
using Domain.UseCase.Accounts;

namespace EntryPoints.ReactiveWeb.Controllers
{
    /// <summary>
    /// Controlador de <see cref="Account"/> implementando <see cref="AccountUseCase"/>
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
    public class AccountController : AppControllerBase<AccountController>
    {
        private readonly ICuentaUseCase _cuentaUseCase;
        private readonly IMapper _mapper;

        /// <summary>
        /// Crea una instancia de <see cref="AccountController"/>
        /// </summary>
        /// <param name="eventsService"></param>
        /// <param name="cuentaUseCase"></param>
        /// <param name="mapper"></param>
        public AccountController(IManageEventsUseCase eventsService, ICuentaUseCase cuentaUseCase, IMapper mapper) :
            base(eventsService)
        {
            _cuentaUseCase = cuentaUseCase;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint que retorna una entidad de tipo <see cref="Account"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public Task<IActionResult> FindAccountById(string id) => HandleRequest(async () =>
            await _cuentaUseCase.FindAccountById(id), "");

        /// <summary>
        /// Endpoint para crear entidad de tipo <see cref="Account"/>
        /// </summary>
        /// <param name="crearCuenta"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{idUsuario}")]
        public Task<IActionResult> Create(string idUsuario, [FromBody] CrearCuenta crearCuenta) => HandleRequest(
            async () =>
            {
                Account accountMapeada = _mapper.Map<Account>(crearCuenta);
                Account account = await _cuentaUseCase.Create(idUsuario, accountMapeada);
                return _mapper.Map<CuentaHandler>(account);
            }, "");

        /// <summary>
        /// Endpoint para crear entidad de tipo <see cref="Account"/>
        /// </summary>
        /// <param name="estadoCuenta"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idUsuario}")]
        public Task<IActionResult> Cancel(string idUsuario, [FromBody] EstadosCuenta estadoCuenta) => HandleRequest(
            async () =>
            {
                Account accountMapeada = _mapper.Map<Account>(estadoCuenta);
                Account account = await _cuentaUseCase.CancelAccount(idUsuario, accountMapeada);
                return _mapper.Map<CuentaHandler>(account);
            }, "");

        /// <summary>
        /// Endpoint para crear entidad de tipo <see cref="Account"/>
        /// </summary>
        /// <param name="estadoCuenta"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idUsuario}")]
        public Task<IActionResult> EnableAccount(string idUsuario, [FromBody] EstadosCuenta estadoCuenta) => HandleRequest(
            async () =>
            {
                Account accountMapeada = _mapper.Map<Account>(estadoCuenta);
                Account account = await _cuentaUseCase.EnableAccount(idUsuario, accountMapeada);
                return _mapper.Map<CuentaHandler>(account);
            }, "");

        /// <summary>
        /// Endpoint para crear entidad de tipo <see cref="Account"/>
        /// </summary>
        /// <param name="estadoCuenta"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idUsuario}")]
        public Task<IActionResult> DisableAccount(string idUsuario, [FromBody] EstadosCuenta estadoCuenta) =>
            HandleRequest(async () =>
            {
                Account accountMapeada = _mapper.Map<Account>(estadoCuenta);
                Account account = await _cuentaUseCase.DisableAccount(idUsuario, accountMapeada);
                return _mapper.Map<CuentaHandler>(account);
            }, "");

        /// <summary>
        /// Endpoint para obtener todas las entidades tipo <see cref="Account"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> FindAll() =>
            await HandleRequest(async () => await _cuentaUseCase.FindAll(), "");

        /// <summary>
        /// Endpoint para obtener lista de entidades de tipo <see cref="Account"/> por cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idCliente}")]
        public async Task<IActionResult> FindAllByClient(string idCliente) =>
            await HandleRequest(async () => await _cuentaUseCase.FindAllByClient(idCliente), "");
    }
}