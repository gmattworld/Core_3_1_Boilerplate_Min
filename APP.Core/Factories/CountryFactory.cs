using APP.Core.Entities;
using APP.Core.Enums;
using APP.Core.Model;
using APP.Core.Model.ModelMini;
using System;

namespace APP.Core.Factories
{
	/// <summary>
	/// Coountry Factory
	/// </summary>
	public static class CountryFactory
	{
		/// <summary>
		/// Validate Country
		/// </summary>
		/// <param name="model"></param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public static bool ValidateModel(CountryMiniModel model, out string errormsg)
		{
			errormsg = string.Empty;

			if (model == null)
			{
				errormsg = "Country Name and Code Required";
				return false;
			}

			if (string.IsNullOrEmpty(model.Name.Trim()))
			{
				errormsg = "Country Name Required";
				return false;
			}

			if (string.IsNullOrEmpty(model.Code.Trim()))
			{
				errormsg = "Country Code Required";
				return false;
			}
			return true;
		}


		/// <summary>
		/// Validate Country
		/// </summary>
		/// <param name="model"></param>
		/// <param name="id"></param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public static bool ValidateModel(CountryModel model, string id, out string errormsg)
		{
			errormsg = string.Empty;
			if (model == null)
			{
				errormsg = "Country Name and Code Required";
				return false;
			}

			if (String.IsNullOrEmpty(id))
			{
				errormsg = "Country ID Required";
				return false;
			}

			if (model.Id != id)
			{
				errormsg = "ID mismatch";
				return false;
			}

			if (string.IsNullOrEmpty(model.Name.Trim()))
			{
				errormsg = "Country Name Required";
				return false;
			}

			if (string.IsNullOrEmpty(model.Code.Trim()))
			{
				errormsg = "Country Code Required";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Create Country entity
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static Country Create(CountryMiniModel model)
		{
			return new Country()
			{
				Code = model.Code,
				Name = model.Name,

				Id = Guid.NewGuid().ToString(),
				CreatedAt = DateTime.Now,
				IsApproved = true,
				IsRestricted = false,
				ModifiedAt = DateTime.MinValue,
				RecordStatus = RecordStatus.ACTIVE
			};
		}

		/// <summary>
		/// Update Country entity
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static Country Create(CountryModel model)
		{
			return new Country()
			{
				Id = model.Id,
				Code = model.Code,
				Name = model.Name,

				IsApproved = model.IsApproved,
				IsRestricted = model.IsRestricted,
				ModifiedAt = DateTime.Now,
				RecordStatus = model.RecordStatus
			};
		}
	}
}
