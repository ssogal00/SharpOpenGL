#pragma once

#define generic GenericFromFreeTypeLibrary
#include "ft2build.h"
#include "freetype/ftglyph.h"
#undef generic


namespace FreeTypeLibWrapper
{
	public ref class GlyphInfo
	{
	public:
		GlyphInfo(){}
		GlyphInfo(const GlyphInfo% rhs)
			: AdvanceHorizontal(rhs.AdvanceHorizontal),
			AdvanceVertical(rhs.AdvanceVertical),
			Width(rhs.Width),
		Height(rhs.Height),
		HoriBearingX(rhs.HoriBearingX),
		HoriBearingY(rhs.HoriBearingY),
		HoriAdvance(rhs.HoriAdvance),
		AtlasX(rhs.AtlasX),
		AtlasY(rhs.AtlasY),
		TexCoordX(rhs.TexCoordX),
		TexCoordY(rhs.TexCoordY),
		CharCode(rhs.CharCode)
		{			
		}

		float AdvanceHorizontal;
		float AdvanceVertical;
		float TexCoordX;
		float TexCoordY;
		float AtlasX;
		float AtlasY;

		signed long Width;
		signed long Height;
		signed long HoriBearingX;
		signed long HoriBearingY;
		signed long HoriAdvance;
		
		FT_ULong CharCode;
	};
}