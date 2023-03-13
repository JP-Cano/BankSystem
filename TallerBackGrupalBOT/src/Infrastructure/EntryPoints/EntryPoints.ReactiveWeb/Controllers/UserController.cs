using AutoMapper;
using Domain.UseCase.Common;
using EntryPoints.ReactiveWeb.Base;
using EntryPoints.ReactiveWeb.Entities.Commands;
using EntryPoints.ReactiveWeb.Entities.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Users;
using Domain.UseCase.Users;

namespace EntryPoints.ReactiveWeb.Controllers;

/// <summary>
/// Controller de entidad <see cref="User"/>
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/[controller]/[action]")]
public class UserController : AppControllerBase<UserController>
{
    private readonly IUserUseCase _userUseCase;
    private readonly IMapper _mapper;

    /// <summary>
    /// Crea una instancia de <see cref="UserController"/>
    /// </summary>
    /// <param name="eventsService"></param>
    /// <param name="userUseCase"></param>
    /// <param name="mapper"></param>
    public UserController(IManageEventsUseCase eventsService, IUserUseCase userUseCase, IMapper mapper) :
        base(eventsService)
    {
        _userUseCase = userUseCase;
        _mapper = mapper;
    }

    /// <summary>
    /// Endpoint que retorna todas las entidades de tipo <see cref="User"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> FindAll() =>
        HandleRequest(async () =>
        {
            IEnumerable<User> usuarios = await _userUseCase.FindAll();
            return _mapper.Map<IEnumerable<UserHandler>>(usuarios);
        }, "");

    /// <summary>
    /// Endpoint que retorna una entidad de tipo <see cref="User"/> por su Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> FindById([FromRoute] string id) =>
        HandleRequest(async () => await _userUseCase.FindById(id), "");

    /// <summary>
    /// Endpoint para crear entidad de tipo <see cref="User"/>
    /// </summary>
    /// <param name="createUser"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateUser createUser) => HandleRequest(async () =>
            {
                User userCreado = await _userUseCase.Create(_mapper.Map<User>(createUser));

                return _mapper.Map<UserHandler>(userCreado);
            }, "");
}