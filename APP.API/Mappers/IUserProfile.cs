using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Repository.EFRepo.EntitiesExt;

namespace APP.API.Mappers
{
    public class IUserProfile : AutoMapper.Profile
    {
        public IUserProfile()
        {
            CreateMap<IUser, UserModel>();
            CreateMap<IUser, UsersExtModel>();
            CreateMap<IUser, UsersExtModel_Auth>();
            CreateMap<UserModel, IUser>();
            // For member can be used for custom mapping
        }
    }
}
