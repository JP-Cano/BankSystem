using AutoMapper;
using Domain.Model.Entities.Clientes;
using Domain.UseCase.Common;
using EntryPoints.ReactiveWeb.Base;
using EntryPoints.ReactiveWeb.Entities.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Model.Entities.Clients;
using Domain.UseCase.Clients;

namespace EntryPoints.ReactiveWeb.Controllers
{
    /// <summary>
    /// Controlador de <see cref="Client"/> implementando <see cref="ClientUseCase"/>
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/clientes")]
    public class ClientController : AppControllerBase<ClientController>
    {
        private readonly IClientUseCase _useCase;

        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor de <see cref="ClientController"/>
        /// </summary>
        /// <param name="eventsUseCase"></param>
        /// <param name="useCase"></param>
        /// <param name="mapper"></param>
        public ClientController(IManageEventsUseCase eventsUseCase, IClientUseCase useCase, IMapper mapper) :
            base(eventsUseCase)
        {
            _useCase = useCase;
            _mapper = mapper;
        }

        /// <summary>
        /// <see cref="ClientUseCase.UpdateEmail"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="nuevoCorreo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/email/{idCliente}/{nuevoCorreo}")]
        public Task<IActionResult> UpdateClientEmail(string idCliente, string nuevoCorreo) =>
            HandleRequest(async () => await _useCase.UpdateEmail(idCliente, nuevoCorreo), "");

        /// <summary>
        /// <see cref="ClientUseCase.CreateClient"/>
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="nuevoCliente"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add/{idUsuario}")]
        public Task<IActionResult> CreateClient(string idUsuario, CreateClient nuevoCliente) =>
            HandleRequest(async () => await _useCase.CreateClient(idUsuario, _mapper.Map<Client>(nuevoCliente)), "");

        /// <summary>
        /// <see cref="ClientUseCase.DisableClient"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("del/{idCliente}")]
        public Task<IActionResult> DisableClient(string idCliente) =>
            HandleRequest(async () => await _useCase.DisableClient(idCliente), "");

        /// <summary>
        /// <see cref="ClientUseCase.DisableClientDebt"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("del/deuda/{idCliente}")]
        public Task<IActionResult> DisableClientDebt(string idCliente) =>
            HandleRequest(async () => await _useCase.DisableClientDebt(idCliente), "");

        /// <summary>
        /// <see cref="ClientUseCase.EnableClient"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("enable/{idCliente}")]
        public Task<IActionResult> EnableClient(string idCliente) =>
            HandleRequest(async () => await _useCase.EnableClient(idCliente), "");

        /// <summary>
        /// <see cref="ClientUseCase.EnableClientDebt"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("enable/deuda/{idCliente}")]
        public Task<IActionResult> EnableClientDebt(string idCliente) =>
            HandleRequest(async () => await _useCase.EnableClientDebt(idCliente), "");

        /// <summary>
        /// <see cref="ClientUseCase.FindClientById"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idCliente}")]
        public Task<IActionResult> FindClientById(string idCliente) =>
                HandleRequest(async () => await _useCase.FindClientById(idCliente), "");

        /// <summary>
        /// <see cref="ClientUseCase.FindAll"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IActionResult> FindAll() =>
            HandleRequest(async () => await _useCase.FindAll(), "");
    }
}