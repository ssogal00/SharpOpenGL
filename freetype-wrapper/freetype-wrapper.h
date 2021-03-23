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
	};
	
	public ref class FreeType
	{
	public:
		static array<unsigned char>^ GetFontAtlas(System::String^ fontPath, System::String^ charSet, int pixelSize);
	protected:

	private:
	};
};
