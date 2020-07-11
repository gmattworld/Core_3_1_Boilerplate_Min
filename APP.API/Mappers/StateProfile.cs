using APP.Core.Entities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Model.ModelMini;

namespace APP.API.Mappers
{
    public class StateProfile : AutoMapper.Profile
    {
        public StateProfile()
        {
            CreateMap<State, StateModel>().ReverseMap();
            CreateMap<State, StateExtModel>().ReverseMap();
            CreateMap<State, StateMiniModel>().ReverseMap();
            // For member can be used for custom mapping
        }
    }
}
