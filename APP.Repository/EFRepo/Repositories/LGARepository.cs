using APP.Core.Entities;
using APP.Core.Enums;
using APP.Repository.IRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.Repositories
{
    public class LGARepository : BaseRepository<LGA, string>, ILGARepository
    {
        public LGARepository(AppDBContext context) : base(context) { }

        public bool CheckExist(string name, string code, State theState, out string errorMsg, string id = null)
        {
            errorMsg = string.Empty;

            if (CheckName(name, theState, id))
            {
                errorMsg = string.Format("Name: {0} exist: ", name);
                return true;
            }

            if (CheckCode(code, theState, id))
            {
                errorMsg = string.Format("Code: {0} exist: ", code);
                return true;
            }

            return false;
        }

        public async Task<LGA> UpdateLGARestrictions(string id)
        {
            LGA _lga = await GetAsync(id);
            if (_lga != null)
            {
                _lga.IsRestricted = !_lga.IsRestricted;
                await UpdateAsync(_lga);
            }

            return _lga;
        }

        public async Task<LGA> DeleteLGA(string id)
        {
            LGA _lga = await GetAsync(id);
            if (_lga != null)
            {
                _lga.RecordStatus = RecordStatus.DELETED;
                await UpdateAsync(_lga);
            }

            return _lga;
        }

        private bool CheckName(string name, State theState, string id)
        {
            if (!id.Equals(0))
            {
                return Find(ex => ex.Name.ToLower().Equals(name.ToLower()) && ex.TheState.Equals(theState) && !ex.Id.Equals(id)).Any();
            }
            return Find(ex => ex.Name.ToLower().Equals(name.ToLower()) && ex.TheState.Equals(theState)).Any();
        }

        private bool CheckCode(string code, State theState, string id)
        {
            if (!id.Equals(0))
            {
                return Find(ex => ex.Code.ToLower().Equals(code.ToLower()) && ex.TheState.Equals(theState) && !ex.Id.Equals(id)).Any();
            }
            return Find(ex => ex.Code.ToLower().Equals(code.ToLower()) && ex.TheState.Equals(theState)).Any();
        }


        public List<LGA> Search(string name, string code, State theState, RecordStatus? status)
        {
            var recordStatus = status == null ? -1 : (int)status;

            var table = Query();
            var query = from a in table
                        where (string.IsNullOrEmpty(name) || a.Name.ToLower().Contains(name.ToLower()))
                              && (string.IsNullOrEmpty(code) || a.Code.ToLower().Contains(code.ToLower()))
                              && (theState == null || a.TheState.Equals(theState))
                              && (recordStatus < 0 || a.RecordStatus == (RecordStatus)recordStatus)
                        select a;
            return query.ToList<LGA>();
        }
    }
}
