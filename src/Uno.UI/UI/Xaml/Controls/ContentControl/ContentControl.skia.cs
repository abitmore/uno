﻿using Uno.Extensions;
using Uno.Foundation.Logging;
using Uno.UI.DataBinding;
using Uno.UI.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using Uno.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using Uno.UI;

namespace Microsoft.UI.Xaml.Controls
{
	public partial class ContentControl
	{
		partial void RegisterContentTemplateRoot()
		{
			AddChild(ContentTemplateRoot);
		}

		partial void UnregisterContentTemplateRoot()
		{
			RemoveChild(ContentTemplateRoot);
		}
	}
}
