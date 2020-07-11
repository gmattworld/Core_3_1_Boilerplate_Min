using APP.Core.Entities;
using APP.Core.Enums;
using APP.Repository.IRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.Repositories
{
    public class ProfileRepository : BaseRepository<Profile, string>, IProfileRepository
    {
        public ProfileRepository(AppDBContext context) : base(context) { }

        public bool CheckExist(string email, string username, out string errorMsg, string Id = null)
        {
            errorMsg = string.Empty;

            if (CheckEmailExist(email, Id))
            {
                errorMsg = string.Format("Email Exist");
                return true;
            }

            if (CheckUsernameExist(username, Id))
            {
                errorMsg = string.Format("Username taken");
                return true;
            }

            return false;
        }


        public async Task<Profile> DeleteProfile(string id)
        {
            Profile _profile = await GetAsync(id);
            if (_profile != null)
            {
                _profile.RecordStatus = RecordStatus.DELETED;
                await UpdateAsync(_profile);
            }

            return _profile;
        }


        private bool CheckEmailExist(string email, string Id)
        {
            if (!Id.Equals(0))
            {
                return Find(ex => ex.TheUser.Email.ToLower().Equals(email.ToLower()) && !ex.Id.Equals(Id)).Any();
            }
            return Find(ex => ex.TheUser.Email.ToLower().Equals(email.ToLower())).Any();
        }

        private bool CheckUsernameExist(string username, string Id)
        {
            if (!Id.Equals(0))
            {
                return Find(ex => ex.TheUser.UserName.ToLower().Equals(username.ToLower()) && !ex.Id.Equals(Id)).Any();
            }
            return Find(ex => ex.TheUser.UserName.ToLower().Equals(username.ToLower())).Any();
        }

        public List<Profile> Search(string name, string email, Country theCountry, State theState, LGA theLGA, RecordStatus? status)
        {
            var _recordStatus = status == null ? -1 : (int)status;

            var table = Query();

            var query = from a in table
                        where (theLGA == null || a.TheLGA.Equals(theLGA))
                              && (theState == null || a.TheLGA.TheState.Equals(theState))
                              && (theCountry == null || a.TheLGA.TheState.TheCountry.Equals(theCountry))
                              && (string.IsNullOrEmpty(name) || a.LastName.ToLower().Contains(name.ToLower()))
                              && (string.IsNullOrEmpty(name) || a.FirstName.ToLower().Contains(name.ToLower()))
                              && (string.IsNullOrEmpty(name) || a.TheUser.UserName.ToLower().Contains(name.ToLower()))
                              && (string.IsNullOrEmpty(email) || a.TheUser.Email.ToLower().Contains(email.ToLower()))
                              && (_recordStatus < 0 || a.RecordStatus == (RecordStatus)_recordStatus)
                        select a;
            return query.ToList<Profile>();
        }
    }
}