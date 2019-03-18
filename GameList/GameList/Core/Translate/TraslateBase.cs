using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;

namespace GameList.Core.Translate
{
	public class TraslateBase
	{
		public string Text { get; set; }
		const string ResourceId = "GameList.Resx.AppRes";

		protected static readonly Lazy<ResourceManager> ResMgr =
			new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(TranslateExtension)).Assembly));

		protected string GetText(bool isUpper = false)
		{
			if (Text == null)
				return null;
			string result = ResMgr.Value.GetString(Text);
			if (isUpper)
				return result.ToUpper();
			else
			{
				return result;
			}

		}
	}
}
