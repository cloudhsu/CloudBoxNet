using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBox.Controller
{
	public interface IPropertyDialogPage
	{
		void BeforeDeactivated(object dataObject);
		void BeforeActivated(object dataObject);
	}
}
