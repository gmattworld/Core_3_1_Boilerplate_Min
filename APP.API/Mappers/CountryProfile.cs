using APP.Core.Entities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Model.ModelMini;
using System.Collections.Generic;

namespace APP.API.Mappers
{
    public class CountryProfile : AutoMapper.Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryModel>().ReverseMap();
            CreateMap<List<Country>, List<CountryModel>>().ReverseMap();
            CreateMap<Country, CountryExtModel>().ReverseMap();
            CreateMap<Country, CountryMiniModel>().ReverseMap();
            // For member can be used for custom mapping
        }
    }
}
