using AutoMapper;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Users;
using Domain.UseCase.Clients;
using Domain.UseCase.Users;
using EntryPoints.GRPc.Dtos;
using EntryPoints.GRPc.Protos;
using Grpc.Core;

namespace EntryPoints.GRPc.RPCs;

public class ClientesRpcServices : ClienteServices.ClienteServicesBase
{
    private readonly IUserUseCase _userUseCase;
    private readonly IClientUseCase _clientUseCase;
    private readonly IMapper _mapper;

    public ClientesRpcServices(IUserUseCase userUseCase, IClientUseCase clientUseCase, IMapper mapper)
    {
        _userUseCase = userUseCase;
        _clientUseCase = clientUseCase;
        _mapper = mapper;
    }

    public override async Task<UsuarioProto> CrearUsuario(CrearUsuarioRequest request, ServerCallContext context)
    {
        CrearUsuarioProto usuarioDto = _mapper.Map<CrearUsuarioProto>(request);
        User userARetornar = await _userUseCase.Create(_mapper.Map<User>(usuarioDto));
        return _mapper.Map<UsuarioProto>(userARetornar);
    }

    public override async Task<ClienteProto> CrearCliente(ClienteCrearRequest request, ServerCallContext context)
    {
        var clienteDto = _mapper.Map<Client>(request.Cliente);
        var clienteARetornar = await _clientUseCase.CreateClient(request.IdUsuario, clienteDto);
        return _mapper.Map<ClienteProto>(clienteARetornar);
    }

    public override async Task<RespuestaListaClientes> ObtenerTodosLosClientes(Empty request, ServerCallContext context)
    {
        var respuesta = new RespuestaListaClientes();
        var clientes = await _clientUseCase.FindAll();
        List<ClienteProto> clienteProtos = clientes.Select(cliente => _mapper.Map<ClienteProto>(cliente))
            .ToList();
        respuesta.Clientes.AddRange(clienteProtos);

        return respuesta;
    }
}