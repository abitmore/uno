﻿using Android.Graphics;
using Rect = Windows.Foundation.Rect;

namespace Windows.UI.Xaml.Media;

partial class RevealBrush
{
	protected override Paint GetPaintInner(Rect destinationRect)
	{
		var color = this.IsDependencyPropertySet(FallbackColorProperty) ?
			GetColorWithOpacity(FallbackColor) :
			GetColorWithOpacity(Color);
		return new Paint() { Color = color, AntiAlias = true };
	}
}
