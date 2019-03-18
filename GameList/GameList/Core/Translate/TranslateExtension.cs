using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameList.Core.Translate
{
	[ContentProperty("Text")]
	public class TranslateExtension : TraslateBase,IMarkupExtension
	{
	    public object ProvideValue(IServiceProvider serviceProvider)
	    {
			return GetText();
		}
    }
}
