using APP.Core.Entities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;

namespace APP.Core.Factories
{
	public class UsersFactory : BaseFactory<User, UserModel, UsersExtModel>
	{
		//public bool ValidateModel(UserModel model, bool isUpdate, string updateId, out string errormsg)
		//{
		//	errormsg = string.Empty;
		//	if (isUpdate)
		//	{
		//		if (string.IsNullOrEmpty(updateId))
		//		{
		//			errormsg = "User ID Required";
		//			return false;
		//		}

		//		if (model.Id != updateId)
		//		{
		//			errormsg = "ID mismatch";
		//			return false;
		//		}
		//	}

		//	if (model == null)
		//	{
		//		errormsg = "Object can't be null";
		//		return false;
		//	}

		//	if (string.IsNullOrEmpty(model.Email.Trim()))
		//	{
		//		errormsg = "Email is Required";
		//		return false;
		//	}

		//	if (string.IsNullOrEmpty(model.UserName.Trim()))
		//	{
		//		errormsg = "Username is Required";
		//		return false;
		//	}
		//	return true;

		//}
	}
}
