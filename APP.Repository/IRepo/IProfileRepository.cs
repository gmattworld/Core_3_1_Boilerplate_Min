using APP.Core.Entities;
using APP.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.Repository.IRepo
{
    public interface IProfileRepository : IBaseRepository<Profile, string>
    {
        bool CheckExist(string email, string username, out string errorMsg, string id = null);
        Task<Profile> DeleteProfile(string id);
        List<Profile> Search(string name, string email, Country theCountry, State theState, LGA theLGA, RecordStatus? status);
    }
}