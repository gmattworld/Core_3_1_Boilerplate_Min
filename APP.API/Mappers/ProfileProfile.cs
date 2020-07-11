using APP.Core.Entities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Model.ModelMini;

namespace APP.API.Mappers
{
    public class ProfileProfile : AutoMapper.Profile
    {
        public ProfileProfile()
        {
            CreateMap<Profile, ProfileModel>().ReverseMap();
            CreateMap<Profile, ProfileExtModel>().ReverseMap();
            CreateMap<Profile, ProfileMiniModel>().ReverseMap();
            // For member can be used for custom mapping
        }
    }
}
