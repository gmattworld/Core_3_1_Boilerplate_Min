using APP.Core.Entities;
using APP.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.Repository.IRepo
{
    public interface ICountryRepository : IBaseRepository<Country, string>
    {
        bool CheckExist(string name, string code, out string errorMsg, string id = null);
        Task<Country> UpdateCountryRestrictions(string id);
        Task<Country> DeleteCountry(string id);
        List<Country> Search(string name, string code, RecordStatus? status);
    }
}