#pragma once



using namespace System::Collections::Generic;

namespace FreeTypeWrapper
{
	public ref class Glyph
	{
	public:
		int AdvanceX;
		int AdvanceY;
		int BitmapWidth;
		int BitmapHeight;
		int BitmapLeft;
		int BitmapTop;
		float TexCoordLeftX;
		float TexCoordRightX;
		float TexCoordTopY;
		float TexCoordBottomY;
	};

	public ref class FontAtlas
	{
	public:
		FontAtlas(array<unsigned char>^ bitmap, Dictionary<wchar_t, Glyph^>^ glyphMap, int squareSize);

		int SquarePixelSize;

		array<unsigned char>^ Bitmap;

		Dictionary<wchar_t, Glyph^>^ GlyphMap;
	};
	
	public ref class FreeType
	{
	public:
		static FontAtlas^ GetFontAtlas(System::String^ fontPath, System::String^ charSet, int pixelSize);
	protected:

	private:
	};
};
