using APP.Core.Entities;
using APP.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.Repository.IRepo
{
    public interface IStateRepository : IBaseRepository<State, string>
    {
        bool CheckExist(string name, string code, Country theCountry, out string errorMsg, string id = null);
        Task<State> UpdateStateRestrictions(string id);
        Task<State> DeleteState(string id);
        List<State> Search(string name, string code, Country theCountry, RecordStatus? status);
    }
}