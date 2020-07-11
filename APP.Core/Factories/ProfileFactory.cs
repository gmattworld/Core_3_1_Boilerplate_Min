using APP.Core.Entities;
using APP.Core.Enums;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Model.ModelMini;
using System;

namespace APP.Core.Factories
{
	public class ProfileFactory
    {
        /// <summary>
        /// Validate Profile
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ValidateModel(ProfileMiniModel model, out string errormsg)
        {
            errormsg = string.Empty;

            if (model == null)
            {
                errormsg = "Profile Name and description Required";
                return false;
            }

            if (string.IsNullOrEmpty(model.FirstName.Trim()))
            {
                errormsg = "Profile Name Required";
                return false;
            }

            //if (string.IsNullOrEmpty(model.Gender.Trim()))
            //{
            //    errormsg = "Profile Name Required";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(model.Description.Trim()))
            //{
            //    errormsg = "Profile description Required";
            //    return false;
            //}
            return true;
        }


        /// <summary>
        /// Validate Profile
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ValidateModel(ProfileModel model, string id, out string errormsg)
        {
            errormsg = string.Empty;
            if (model == null)
            {
                errormsg = "Profile Name and description Required";
                return false;
            }

            if (String.IsNullOrEmpty(id))
            {
                errormsg = "Profile ID Required";
                return false;
            }

            if (model.Id != id)
            {
                errormsg = "ID mismatch";
                return false;
            }

            //if (string.IsNullOrEmpty(model.Name.Trim()))
            //{
            //    errormsg = "Profile Name Required";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(model.Description.Trim()))
            //{
            //    errormsg = "Profile description required";
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// Create Profile entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Profile Create(ProfileMiniModel model, LGA theLGA, User theUser)
        {
            return new Profile()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Hobby = model.Hobby,
                Profession = model.Profession,
                MaritalStatus = model.MaritalStatus,
                Gender = model.Gender,
                TheLGA = theLGA,
                TheUser = theUser,

                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                IsApproved = true,
                ModifiedAt = DateTime.MinValue,
                RecordStatus = RecordStatus.ACTIVE
            };
        }

        /// <summary>
        /// Update Profile entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Profile Create(ProfileModel model, LGA theLGA, User theUser)
        {
            return new Profile()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Hobby = model.Hobby,
                Profession = model.Profession,
                MaritalStatus = model.MaritalStatus,
                Gender = model.Gender,
                TheLGA = theLGA,
                TheUser = theUser,

                IsApproved = model.IsApproved,
                ModifiedAt = DateTime.Now,
                RecordStatus = model.RecordStatus
            };
        }
    }
}