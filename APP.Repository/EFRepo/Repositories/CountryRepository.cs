using APP.Core.Entities;
using APP.Core.Enums;
using APP.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.Repositories
{
    public class CountryRepository : BaseRepository<Country, string>, ICountryRepository
    {
        public CountryRepository(AppDBContext context) : base(context) { }

        /// <summary>
        /// This check if the entity exist, It return true if it exists
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="errorMsg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExist(string name, string code, out string errorMsg, string id = null)
        {
            errorMsg = string.Empty;

            if (CheckName(name, id))
            {
                errorMsg = string.Format("Name: {0} exist: ", name);
                return true;
            }

            if (CheckCode(code, id))
            {
                errorMsg = string.Format("Code: {0} exist: ", code);
                return true;
            }

            return false;
        }

        public async Task<Country> UpdateCountryRestrictions(string id)
        {
            Country _country = await GetAsync(id);
            if (_country != null)
            {
                _country.IsRestricted = !_country.IsRestricted;
                await UpdateAsync(_country);
            }

            return _country;
        }

        public async Task<Country> DeleteCountry(string id)
        {
            Country _country = await GetAsync(id);
            if (_country != null)
            {
                _country.RecordStatus = RecordStatus.DELETED;
                await UpdateAsync(_country);
            }

            return _country;
        }

        private bool CheckName(string name, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return Find(ex => ex.Name.ToLower().Equals(name.ToLower()) && !ex.Id.Equals(id)).Any();
            }
            return Find(ex => ex.Name.ToLower().Equals(name.ToLower())).Any();
        }

        private bool CheckCode(string code, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return Find(ex => ex.Code.ToLower().Equals(code.ToLower()) && !ex.Id.Equals(id)).Any();
            }
            return Find(ex => ex.Code.ToLower().Equals(code.ToLower())).Any();
        }

        public List<Country> Search(string name, string code, RecordStatus? status)
        {
            var recordStatus = status == null ? -1 : (int)status;

            var table = Query();
            var query = from a in table
                        where (string.IsNullOrEmpty(name) || a.Name.ToLower().Contains(name.ToLower()))
                              && (string.IsNullOrEmpty(code) || a.Code.ToLower().Contains(code.ToLower()))
                              && (recordStatus < 0 || a.RecordStatus == (RecordStatus)recordStatus)
                        select a;
            return query.ToList<Country>();
        }
    }
}
