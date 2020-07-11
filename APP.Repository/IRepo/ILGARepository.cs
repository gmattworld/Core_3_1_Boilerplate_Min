using APP.Core.Entities;
using APP.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.Repository.IRepo
{
    public interface ILGARepository : IBaseRepository<LGA, string>
    {
        bool CheckExist(string name, string code, State theState, out string errorMsg, string id = null);
        Task<LGA> UpdateLGARestrictions(string id);
        Task<LGA> DeleteLGA(string id);
        List<LGA> Search(string name, string code, State theState, RecordStatus? status);
    }
}