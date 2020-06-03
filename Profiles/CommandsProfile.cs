using AutoMapper;
using MVC_REST_API.Dtos;
using MVC_REST_API.Models;

namespace MVC_REST_API.Profiles
{
    public class CommandsProfile : Profile
    {
        // this is being used in the background to map different objects
        // from source to target
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<CommanderUpdateDto, Command>();
            CreateMap<Command, CommanderUpdateDto>();
        }
    }
}
