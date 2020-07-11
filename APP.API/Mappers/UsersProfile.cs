using APP.Core.Entities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;

namespace APP.API.Mappers
{
    public class UsersProfile : AutoMapper.Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<User, UsersExtModel>();
            CreateMap<User, UsersExtModel_Auth>();
            CreateMap<UserModel, User>();
            // For member can be used for custom mapping
        }
    }
}
