#include "pch.h"
#include "freetype-wrapper.h"
#define generic __identifier(generic)
#include "ft2build.h"
#include "freetype/freetype.h"
#undef generic
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <cmath>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
#using <mscorlib.dll>

array<unsigned char>^ FreeTypeWrapper::FreeType::GetFontAtlas(System::String^ fontPath, System::String^ charSet, int pixelSize)
{
	FT_Library lib;

	FT_Init_FreeType(&lib);

	FT_Face face;

	FT_New_Face(lib, "C:\\windows\\fonts\\batang.ttc", 2, &face);

	int length = charSet->Length;

	int squareSize = ceil(sqrt(length));	

	std::wstring charSets = marshal_as<std::wstring>(charSet);

	array<uint8_t>^ resultPixels = gcnew array<uint8_t>((pixelSize * squareSize) * (pixelSize*squareSize));

	int error;

	error = FT_Set_Pixel_Sizes(face, pixelSize, pixelSize);


	for(int i = 0; i < length; ++i)
	{
		int col = i % squareSize;
		int row = i / squareSize;
		
		unsigned int charIndex = FT_Get_Char_Index(face, charSets[i]);
		
		error = FT_Load_Glyph(face, charIndex, FT_LOAD_DEFAULT | FT_LOAD_NO_BITMAP);

		error = FT_Render_Glyph(face->glyph, FT_RENDER_MODE_NORMAL);

		int dstStartIndex = (pixelSize*pixelSize*squareSize) * row + (pixelSize * pixelSize) * col;

		for (int y = 0; y < face->glyph->bitmap.rows; y++)
		{
			for (int x = 0; x < face->glyph->bitmap.width; x++)
			{
				int dstIndex = pixelSize *(y)+(x) + dstStartIndex;
				int srcIndex = y * face->glyph->bitmap.width + x;
				resultPixels[dstIndex] = face->glyph->bitmap.buffer[srcIndex];
			}
		}		
	}	
		
	FT_Done_FreeType(lib);

	return resultPixels;
}
 