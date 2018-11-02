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
		float AdvanceHorizontal;
		float AdvanceVertical;
		float TexCoordX;
		float TexCoordY;
		float AtlasX;
		float AtlasY;
		FT_ULong CharCode;
	};
}