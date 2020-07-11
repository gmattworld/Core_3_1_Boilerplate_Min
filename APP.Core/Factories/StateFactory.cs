using APP.Core.Entities;
using APP.Core.Enums;
using APP.Core.Model;
using APP.Core.Model.ModelMini;
using System;

namespace APP.Core.Factories
{
    /// <summary>
    /// State Factory
    /// </summary>
    public static class StateFactory
    {
        /// <summary>
        /// Validate State
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ValidateModel(StateMiniModel model, out string errormsg)
        {
            errormsg = string.Empty;

            if (model == null)
            {
                errormsg = "State Name and Code Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                errormsg = "State Name Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.Code.Trim()))
            {
                errormsg = "State Code Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.CountryId.Trim()))
            {
                errormsg = "Country Required";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate State
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ValidateModel(StateModel model, string id, out string errormsg)
        {
            errormsg = string.Empty;
            if (model == null)
            {
                errormsg = "State Name and Code Required";
                return false;
            }

            if (String.IsNullOrEmpty(id))
            {
                errormsg = "State ID Required";
                return false;
            }

            if (model.Id != id)
            {
                errormsg = "ID mismatch";
                return false;
            }

            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                errormsg = "State Name Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.Code.Trim()))
            {
                errormsg = "State Code Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.CountryId.Trim()))
            {
                errormsg = "Country Required";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Create State entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="theCountry"></param>
        /// <returns></returns>
        public static State Create(StateMiniModel model, Country theCountry)
        {
            return new State()
            {
                Code = model.Code,
                Name = model.Name,
                TheCountry = theCountry,

                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                IsApproved = true,
                IsRestricted = false,
                ModifiedAt = DateTime.MinValue,
                RecordStatus = RecordStatus.ACTIVE
            };
        }

        /// <summary>
        /// Update State entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="theCountry"></param>
        /// <returns></returns>
        public static State Create(StateModel model, Country theCountry)
        {
            return new State()
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                TheCountry = theCountry,

                IsApproved = model.IsApproved,
                IsRestricted = model.IsRestricted,
                ModifiedAt = DateTime.Now,
                RecordStatus = model.RecordStatus
            };
        }
    }
}
