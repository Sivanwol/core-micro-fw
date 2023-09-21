using Application.Responses.Users;
using AutoMapper;
using Processor.Consumers.IndexUser;
using Processor.Handlers.User.Create;
using Processor.Handlers.User.List;

namespace Processor; 

public class DomainProcessorProfile : Profile {
    public DomainProcessorProfile() {
        userMapping();
    }
    
    private void userMapping() {
        // user create mapping
        CreateMap<Domain.Entities.User, CreateUserRequest>();
        // index user event mapping
        CreateMap<Domain.Entities.User, IndexUserEvent>();
        // user list mapping
        CreateMap<Domain.Entities.User, ListUsersRequest>();
        CreateMap<Domain.Entities.User, ListUserResponse>();
    }
}