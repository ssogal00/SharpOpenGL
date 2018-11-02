#pragma once
#define generic GenericFromFreeTypeLibrary
#include "ft2build.h"
#include "freetype/ftglyph.h"
#include "GlyphInfo.h"
#undef generic

using namespace System::Collections::Generic;

namespace FreeTypeLibWrapper
{
	public ref class FreeType
	{
	public:
		bool Initialize(System::String^ filePath);
		List<unsigned char>^ GetTextureData() { return textureData; }
	private:
		bool GetCorrectResolution(int resolution, int numglyph, int& outNewResolution, int& outNewMargin);
		int NextPowerOf2(const int n);
		void SetPixel(int offset, int size, int x, int y, unsigned char val);
		void GenerateTextureFromGlyph(FT_GlyphSlot glyph, int atlasX, int atlasY, int texSize, int resolution, int marginSize, bool drawBorder);

		Dictionary<unsigned long, GlyphInfo^> glyphMap = gcnew Dictionary<unsigned long, GlyphInfo^>();
		List<unsigned char>^ textureData = gcnew List<unsigned char>();

	};
}