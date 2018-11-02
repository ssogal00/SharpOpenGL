#pragma once

#include <cmath>

namespace FreeTypeLibWrapper
{
	public ref class GlyphBox
	{
	public:
		GlyphBox()			
		{			
		}

		GlyphBox(long xMin, long xMax, long yMin, long yMax)
			: XMin(xMin), XMax(xMax), YMin(yMin), YMax(yMax)
		{			
		}

		long XMin = LONG_MAX;
		long XMax = LONG_MIN;
		long YMin = LONG_MAX;
		long YMax = LONG_MIN;
	}
}