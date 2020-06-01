﻿using Uno.Media;
using Windows.Foundation;

namespace Windows.UI.Xaml.Shapes
{
	public partial class Polyline
	{
		protected override Android.Graphics.Path GetPath(Size availableSize)
		{
			var coords = Points;

			if (coords == null || coords.Count <= 1)
			{
				return null;
			}

			var streamGeometry = GeometryHelper.Build(c =>
			{
				c.BeginFigure(new Point(coords[0].X, coords[0].Y), true);
				for (var i = 1; i < coords.Count; i++)
				{
					c.LineTo(new Point(coords[i].X, coords[i].Y), true, false);
				}
			});

			return streamGeometry.ToPath();

		}
	}
}
