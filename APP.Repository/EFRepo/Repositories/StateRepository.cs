using APP.Core.Entities;
using APP.Core.Enums;
using APP.Repository.IRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.Repositories
{
    public class StateRepository : BaseRepository<State, string>, IStateRepository
    {
        public StateRepository(AppDBContext context) : base(context) { }

        public bool CheckExist(string name, string code, Country theCountry, out string errorMsg, string id = null)
        {
            errorMsg = string.Empty;

            if (CheckName(name, theCountry, id))
            {
                errorMsg = string.Format("Name: {0} exist: ", name);
                return true;
            }

            if (CheckCode(code, theCountry, id))
            {
                errorMsg = string.Format("Code: {0} exist: ", code);
                return true;
            }

            return false;
        }

        public async Task<State> UpdateStateRestrictions(string id)
        {
            State _state = await GetAsync(id);
            if (_state != null)
            {
                _state.IsRestricted = !_state.IsRestricted;
                await UpdateAsync(_state);
            }

            return _state;
        }

        public async Task<State> DeleteState(string id)
        {
            State _state = await GetAsync(id);
            if (_state != null)
            {
                _state.RecordStatus = RecordStatus.DELETED;
                await UpdateAsync(_state);
            }

            return _state;
        }

        private bool CheckName(string name, Country theCountry, string id)
        {
            if (!id.Equals(0))
            {
                return Find(ex => ex.Name.ToLower().Equals(name.ToLower()) && ex.TheCountry.Equals(theCountry) && !ex.Id.Equals(id)).Any();
            }
            return Find(ex => ex.Name.ToLower().Equals(name.ToLower()) && ex.TheCountry.Equals(theCountry)).Any();
        }

        private bool CheckCode(string code, Country theCountry, string id)
        {
            if (!id.Equals(0))
            {
                return Find(ex => ex.Code.ToLower().Equals(code.ToLower()) && ex.TheCountry.Equals(theCountry) && !ex.Id.Equals(id)).Any();
            }
            return Find(ex => ex.Code.ToLower().Equals(code.ToLower()) && ex.TheCountry.Equals(theCountry)).Any();
        }


        public List<State> Search(string name, string code, Country theCountry, RecordStatus? status)
        {
            var recordStatus = status == null ? -1 : (int)status;

            var table = Query();
            var query = from a in table
                        where (string.IsNullOrEmpty(name) || a.Name.ToLower().Contains(name.ToLower()))
                              && (string.IsNullOrEmpty(code) || a.Code.ToLower().Contains(code.ToLower()))
                              && (theCountry == null || a.TheCountry.Equals(theCountry))
                              && (recordStatus < 0 || a.RecordStatus == (RecordStatus)recordStatus)
                        select a;
            return query.ToList<State>();
        }
    }
}
