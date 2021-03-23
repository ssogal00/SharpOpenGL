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

array<unsigned char>^ FreeTypeWrapper::FreeType::GetFontAtlas(System::String^ fontPath, System::String^ charSet)
{
	FT_Library lib;

	FT_Init_FreeType(&lib);

	FT_Face face;

	FT_New_Face(lib, "C:\\windows\\fonts\\batang.ttc", 2, &face);

	unsigned int index = FT_Get_Char_Index(face, TEXT('Çü'));	

	int error;

	error = FT_Set_Pixel_Sizes(face, 128, 128);

	error = FT_Load_Glyph(face, index, FT_LOAD_DEFAULT | FT_LOAD_NO_BITMAP);

	error = FT_Render_Glyph(face->glyph, FT_RENDER_MODE_NORMAL);

	int color;

	int width = face->glyph->bitmap.width;
	int height = face->glyph->bitmap.rows;
	

	array<uint8_t>^ result = gcnew array<uint8_t>(128*128);

	for(int y = 0; y < face->glyph->bitmap.rows; y++)
	{
		for(int x = 0; x < face->glyph->bitmap.width;x++)
		{
			int targetIndex = 128 * y + x;
			int srcIndex = y * width + x;
			result[targetIndex] = face->glyph->bitmap.buffer[srcIndex];
		}
	}
	
	FT_Done_FreeType(lib);
	
	return result;
}
 