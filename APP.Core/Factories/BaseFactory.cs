using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APP.Core.Factories
{
	public class BaseFactory<TEntity, TModel, TExtModel>
	{
		public TEntity Patch(TEntity src, TModel obj, string fields)
		{
			List<string> lstOfFields = new List<string>();
			if (string.IsNullOrEmpty(fields))
			{
				return src;
			}
			lstOfFields = fields.Split(',').ToList();
			List<string> lstOfFieldsToWorkWith = new List<string>(lstOfFields);
			if (!lstOfFieldsToWorkWith.Any())
			{
				return src;
			}
			else
			{
				foreach (var field in lstOfFieldsToWorkWith)
				{

					var fieldProp = obj.GetType()
						.GetProperty(field.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

					var fieldValue = fieldProp.GetValue(obj, null);
					var property = src.GetType()
						.GetProperty(field.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

					Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

					object safeValue = (fieldValue == null) ? null : Convert.ChangeType(fieldValue, t);
					property.SetValue(src, fieldValue, null);
				}
				return src;
			}
		}
	}
}
