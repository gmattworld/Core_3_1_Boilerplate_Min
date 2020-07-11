using APP.Core.Entities;
using APP.Core.Enums;
using APP.Core.Model;
using APP.Core.Model.ModelMini;
using System;

namespace APP.Core.Factories
{
    /// <summary>
    /// LGA Factory
    /// </summary>
	public static class LGAFactory
	{
        /// <summary>
        /// Validate LGA
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ValidateModel(LGAMiniModel model, out string errormsg)
        {
            errormsg = string.Empty;

            if (model == null)
            {
                errormsg = "LGA Name and Code Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                errormsg = "LGA Name Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.Code.Trim()))
            {
                errormsg = "LGA Code Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.StateId.Trim()))
            {
                errormsg = "State Required";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate LGA
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ValidateModel(LGAModel model, string id, out string errormsg)
        {
            errormsg = string.Empty;
            if (model == null)
            {
                errormsg = "LGA Name and Code Required";
                return false;
            }

            if (String.IsNullOrEmpty(id))
            {
                errormsg = "LGA ID Required";
                return false;
            }

            if (model.Id != id)
            {
                errormsg = "ID mismatch";
                return false;
            }

            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                errormsg = "LGA Name Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.Code.Trim()))
            {
                errormsg = "LGA Code Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.StateId.Trim()))
            {
                errormsg = "State Required";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Create LGA entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="theState"></param>
        /// <returns></returns>
        public static LGA Create(LGAMiniModel model, State theState)
        {
            return new LGA()
            {
                Code = model.Code,
                Name = model.Name,
                TheState = theState,

                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                IsApproved = true,
                IsRestricted = false,
                ModifiedAt = DateTime.MinValue,
                RecordStatus = RecordStatus.ACTIVE
            };
        }

        /// <summary>
        /// Update LGA entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="theState"></param>
        /// <returns></returns>
        public static LGA Create(LGAModel model, State theState)
        {
            return new LGA()
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                TheState = theState,

                IsApproved = model.IsApproved,
                IsRestricted = model.IsRestricted,
                ModifiedAt = DateTime.Now,
                RecordStatus = model.RecordStatus
            };
        }
    }
}
