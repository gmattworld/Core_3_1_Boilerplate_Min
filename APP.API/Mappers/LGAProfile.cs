using APP.Core.Entities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Model.ModelMini;

namespace APP.API.Mappers
{
    public class LGAProfile : AutoMapper.Profile
    {
        public LGAProfile()
        {
            CreateMap<LGA, LGAModel>().ReverseMap();
            CreateMap<LGA, LGAExtModel>().ReverseMap();
            CreateMap<LGA, LGAMiniModel>().ReverseMap();
            // For member can be used for custom mapping
        }
    }
}
