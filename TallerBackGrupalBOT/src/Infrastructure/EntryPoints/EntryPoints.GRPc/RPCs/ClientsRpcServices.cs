using AutoMapper;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Users;
using Domain.UseCase.Clients;
using Domain.UseCase.Users;
using EntryPoints.GRPc.Dtos;
using EntryPoints.GRPc.Protos;
using Grpc.Core;

namespace EntryPoints.GRPc.RPCs;

public class ClientesRpcServices : ClientServices.ClientServicesBase
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

    public override async Task<ClientProto> CreateClient(CreateClientRequest request, ServerCallContext context)
    {
        var clientDto = _mapper.Map<Client>(request.Client);
        var returnedClient = await _clientUseCase.CreateClient(request.UserId, clientDto);
        return _mapper.Map<ClientProto>(returnedClient);
    }

    public override async Task<UserProto> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        CreateUserProto userDto = _mapper.Map<CreateUserProto>(request);
        User returnedUser = await _userUseCase.Create(_mapper.Map<User>(userDto));
        return _mapper.Map<UserProto>(returnedUser);
    }

    public override async Task<ClientListResponse> FindAllClients(Empty request, ServerCallContext context)
    {
        var response = new ClientListResponse();
        var clients = await _clientUseCase.FindAll();
        List<ClientProto> clientProto = clients.Select(cliente => _mapper.Map<ClientProto>(cliente))
            .ToList();
        response.Clients.AddRange(clientProto);

        return response;
    }
}